// Server/Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Server.Repositorio;
using GestionDespensa1.Shared.DTO;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthController(
            IUsuarioRepositorio usuarioRepositorio,
            IMapper mapper,
            IConfiguration config)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost("registro")]
        public async Task<ActionResult<LoginRespuestaDTO>> Registro(CrearUsuarioDTO crearUsuarioDTO)
        {
            try
            {
                // 1. Validar que el email no exista
                if (await _usuarioRepositorio.ExisteEmail(crearUsuarioDTO.Email))
                {
                    return BadRequest(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "El email ya está registrado"
                    });
                }

                // 2. Crear el usuario con password hasheado
                var usuario = new Usuario
                {
                    Nombre = crearUsuarioDTO.Nombre,
                    Email = crearUsuarioDTO.Email,
                    Rol = crearUsuarioDTO.Rol,
                    Estado = "ACTIVO",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(crearUsuarioDTO.Password)
                };

                var id = await _usuarioRepositorio.Insert(usuario);

                if (id <= 0)
                {
                    return BadRequest(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "No se pudo crear el usuario"
                    });
                }

                // 3. Generar token
                var token = GenerarToken(usuario);

                return Ok(new LoginRespuestaDTO
                {
                    Success = true,
                    Token = token,
                    Usuario = _mapper.Map<UsuarioDTO>(usuario)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new LoginRespuestaDTO
                {
                    Success = false,
                    Error = $"Error: {ex.Message}"
                });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginRespuestaDTO>> Login(LoginDTO loginDTO)
        {
            try
            {
                // 1. Buscar usuario por email
                var usuario = await _usuarioRepositorio.GetByEmail(loginDTO.Email);

                if (usuario == null)
                {
                    return BadRequest(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "Email o contraseña incorrectos"
                    });
                }

                // 2. Verificar estado
                if (usuario.Estado != "ACTIVO")
                {
                    return BadRequest(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "Usuario inactivo. Contacte al administrador."
                    });
                }

                // 3. Verificar contraseña
                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, usuario.PasswordHash))
                {
                    return BadRequest(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "Email o contraseña incorrectos"
                    });
                }

                // 4. Generar token
                var token = GenerarToken(usuario);

                return Ok(new LoginRespuestaDTO
                {
                    Success = true,
                    Token = token,
                    Usuario = _mapper.Map<UsuarioDTO>(usuario)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new LoginRespuestaDTO
                {
                    Success = false,
                    Error = $"Error: {ex.Message}"
                });
            }
        }

        [HttpGet("validar-token")]
        [Authorize]
        public async Task<ActionResult<LoginRespuestaDTO>> ValidarToken()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return Unauthorized(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "Token inválido"
                    });
                }

                var usuario = await _usuarioRepositorio.GetByEmail(email);
                if (usuario == null || usuario.Estado != "ACTIVO")
                {
                    return Unauthorized(new LoginRespuestaDTO
                    {
                        Success = false,
                        Error = "Usuario no válido"
                    });
                }

                return Ok(new LoginRespuestaDTO
                {
                    Success = true,
                    Usuario = _mapper.Map<UsuarioDTO>(usuario)
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new LoginRespuestaDTO
                {
                    Success = false,
                    Error = $"Error: {ex.Message}"
                });
            }
        }
        // AGREGAR ESTE MÉTODO TEMPORAL EN AuthController.cs
        [HttpGet("crear-usuario-prueba")]
        public async Task<ActionResult<string>> CrearUsuarioPrueba()
        {
            try
            {
                // Verificar si ya existe
                var existe = await _usuarioRepositorio.ExisteEmail("admin@despensa.com");
                if (existe)
                {
                    return Ok("El usuario ya existe");
                }

                // Crear usuario
                var usuario = new Usuario
                {
                    Nombre = "Admin",
                    Email = "admin@despensa.com",
                    Rol = "ADMIN",
                    Estado = "ACTIVO",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456")
                };

                var id = await _usuarioRepositorio.Insert(usuario);

                if (id > 0)
                {
                    return Ok($"Usuario creado correctamente con ID: {id}");
                }

                return BadRequest("No se pudo crear el usuario");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        [HttpGet("crear-usuario-simple")]
        public async Task<ActionResult<string>> CrearUsuarioSimple()
        {
            try
            {
                var usuario = new Usuario
                {
                    Nombre = "Test",
                    Email = "test@test.com",
                    Rol = "ADMIN",
                    Estado = "ACTIVO",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("123")
                };

                var id = await _usuarioRepositorio.Insert(usuario);
                return Ok($"Usuario test creado con ID: {id}");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "MiClaveSuperSecretaParaJWT2026Con32Caracteres!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
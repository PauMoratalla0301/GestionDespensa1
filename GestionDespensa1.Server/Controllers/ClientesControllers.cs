using Microsoft.AspNetCore.Mvc;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Server.Repositorio;
using GestionDespensa1;
using GestionDespensa1.Shared.DTO;
using AutoMapper;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/Clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepositorio _repositorio;
        private readonly IMapper _mapper;

        public ClientesController(IClienteRepositorio repositorio, IMapper mapper)
        {
            _repositorio = repositorio;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> Get()
        {
            var clientes = await _repositorio.SelectWithRelations();
            return _mapper.Map<List<ClienteDTO>>(clientes);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ClienteDTO>> Get(int id)
        {
            var cliente = await _repositorio.SelectByIdWithRelations(id);
            if (cliente == null)
                return NotFound();

            return _mapper.Map<ClienteDTO>(cliente);
        }

        [HttpGet("GetByDni/{dni}")]
        public async Task<ActionResult<ClienteDTO>> GetByDni(string dni)
        {
            var cliente = await _repositorio.GetByDni(dni);
            if (cliente == null)
                return NotFound();

            return _mapper.Map<ClienteDTO>(cliente);
        }

        [HttpGet("GetBySaldoPendiente")]
        public async Task<ActionResult<List<ClienteDTO>>> GetBySaldoPendiente()
        {
            var clientes = await _repositorio.GetBySaldoPendiente();
            return _mapper.Map<List<ClienteDTO>>(clientes);
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            var resultado = await _repositorio.Existe(id);

            if (resultado.Result is BadRequestObjectResult badRequest)
                return BadRequest(badRequest.Value);

            return resultado.Value;
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearClienteDTO crearClienteDTO)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(crearClienteDTO);
                var resultado = await _repositorio.Insert(cliente);

                if (resultado.Result is BadRequestObjectResult badRequest)
                    return BadRequest(badRequest.Value);

                return resultado.Value;
            }
            catch (Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ClienteDTO clienteDTO)
        {
            try
            {
                if (id != clienteDTO.Id)
                    return BadRequest("Datos Incorrectos");

                var cliente = _mapper.Map<Cliente>(clienteDTO);
                var resultado = await _repositorio.Update(id, cliente);

                if (!resultado)
                    return BadRequest("No se pudo actualizar el cliente");

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var resp = await _repositorio.Delete(id);
            if (!resp)
                return BadRequest("El cliente no se pudo borrar");

            return Ok();
        }
    }
}
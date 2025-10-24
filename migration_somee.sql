IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Cajas] (
    [Id] int NOT NULL IDENTITY,
    [IdUsuario] nvarchar(45) NOT NULL,
    [Fecha] datetime2 NOT NULL,
    [ImporteInicio] decimal(18,2) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Cajas] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Categorias] (
    [Id] int NOT NULL IDENTITY,
    [NombreCategoria] nvarchar(45) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Categorias] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Clientes] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(45) NOT NULL,
    [Apellido] nvarchar(45) NOT NULL,
    [Dni] nvarchar(45) NOT NULL,
    [Telefono] nvarchar(45) NULL,
    [Direccion] nvarchar(45) NULL,
    [Email] nvarchar(100) NULL,
    [SaldoPendiente] decimal(18,2) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Clientes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Proveedores] (
    [Id] int NOT NULL IDENTITY,
    [Nombre] nvarchar(45) NOT NULL,
    [CUIT] nvarchar(45) NOT NULL,
    [Telefono] nvarchar(45) NOT NULL,
    [Email] nvarchar(45) NOT NULL,
    [Direccion] nvarchar(45) NOT NULL,
    [Estado] nvarchar(45) NOT NULL,
    [Notas] nvarchar(45) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Proveedores] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [DetallesCaja] (
    [Id] int NOT NULL IDENTITY,
    [IdVenta] nvarchar(45) NOT NULL,
    [Monto] nvarchar(45) NOT NULL,
    [IdCaja] int NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_DetallesCaja] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DetallesCaja_Cajas_IdCaja] FOREIGN KEY ([IdCaja]) REFERENCES [Cajas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Productos] (
    [Id] int NOT NULL IDENTITY,
    [Descripcion] nvarchar(45) NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [GananciaPorcentaje] decimal(5,2) NOT NULL,
    [StockActual] int NOT NULL,
    [StockMinimo] int NOT NULL,
    [IdCategoria] int NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Productos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Productos_Categorias_IdCategoria] FOREIGN KEY ([IdCategoria]) REFERENCES [Categorias] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Ventas] (
    [Id] int NOT NULL IDENTITY,
    [IdCliente] int NOT NULL,
    [FechaVenta] datetime2 NOT NULL,
    [Estado] nvarchar(45) NOT NULL,
    [Total] decimal(18,2) NOT NULL,
    [MontoPagado] decimal(18,2) NOT NULL,
    [SaldoPendiente] decimal(18,2) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_Ventas] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ventas_Clientes_IdCliente] FOREIGN KEY ([IdCliente]) REFERENCES [Clientes] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [ComprasProveedor] (
    [Id] int NOT NULL IDENTITY,
    [IdProveedor] int NOT NULL,
    [FechaCompra] nvarchar(45) NOT NULL,
    [Observaciones] nvarchar(45) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_ComprasProveedor] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ComprasProveedor_Proveedores_IdProveedor] FOREIGN KEY ([IdProveedor]) REFERENCES [Proveedores] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DetallesVenta] (
    [Id] int NOT NULL IDENTITY,
    [IdVenta] int NOT NULL,
    [IdProducto] int NOT NULL,
    [Cantidad] int NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_DetallesVenta] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DetallesVenta_Productos_IdProducto] FOREIGN KEY ([IdProducto]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DetallesVenta_Ventas_IdVenta] FOREIGN KEY ([IdVenta]) REFERENCES [Ventas] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DetallesCompraProveedor] (
    [Id] int NOT NULL IDENTITY,
    [IdCompra] int NOT NULL,
    [IdProducto] int NOT NULL,
    [Cantidad] int NOT NULL,
    [PrecioUnitario] decimal(18,2) NOT NULL,
    [Activo] bit NOT NULL,
    CONSTRAINT [PK_DetallesCompraProveedor] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DetallesCompraProveedor_ComprasProveedor_IdCompra] FOREIGN KEY ([IdCompra]) REFERENCES [ComprasProveedor] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_DetallesCompraProveedor_Productos_IdProducto] FOREIGN KEY ([IdProducto]) REFERENCES [Productos] ([Id]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [Categoria_UQ] ON [Categorias] ([NombreCategoria]);
GO

CREATE UNIQUE INDEX [Cliente_UQ] ON [Clientes] ([Dni]);
GO

CREATE INDEX [IX_ComprasProveedor_IdProveedor] ON [ComprasProveedor] ([IdProveedor]);
GO

CREATE INDEX [IX_DetallesCaja_IdCaja] ON [DetallesCaja] ([IdCaja]);
GO

CREATE INDEX [IX_DetallesCompraProveedor_IdCompra] ON [DetallesCompraProveedor] ([IdCompra]);
GO

CREATE INDEX [IX_DetallesCompraProveedor_IdProducto] ON [DetallesCompraProveedor] ([IdProducto]);
GO

CREATE INDEX [IX_DetallesVenta_IdProducto] ON [DetallesVenta] ([IdProducto]);
GO

CREATE INDEX [IX_DetallesVenta_IdVenta] ON [DetallesVenta] ([IdVenta]);
GO

CREATE INDEX [IX_Productos_IdCategoria] ON [Productos] ([IdCategoria]);
GO

CREATE UNIQUE INDEX [Producto_UQ] ON [Productos] ([Descripcion]);
GO

CREATE UNIQUE INDEX [Proveedor_UQ] ON [Proveedores] ([CUIT]);
GO

CREATE INDEX [IX_Ventas_IdCliente] ON [Ventas] ([IdCliente]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251007074540_NuevaBase', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251011235045_OtraConexion', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251014235651_NuevoInicio', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251015000242_NuevaCadenaCoenxion', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251015011024_NuevaCadena', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251016004215_SomeeConexion', N'8.0.8');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251016005621_SomeeMigrac', N'8.0.8');
GO

COMMIT;
GO


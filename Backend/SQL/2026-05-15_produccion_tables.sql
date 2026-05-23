-- Crear tablas de producción para StockMeal (MySQL)
-- Ejecutar sobre la BD: gestion_stock

USE gestion_stock;

CREATE TABLE IF NOT EXISTS produccion_diaria (
    id_produccion INT AUTO_INCREMENT PRIMARY KEY,
    fecha DATE NOT NULL,
    id_empleado INT NOT NULL,
    observaciones TEXT NULL,
    CONSTRAINT fk_produccion_empleado
        FOREIGN KEY (id_empleado)
        REFERENCES empleado(id_empleado)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE INDEX idx_produccion_fecha ON produccion_diaria(fecha);
CREATE INDEX idx_produccion_empleado ON produccion_diaria(id_empleado);

CREATE TABLE IF NOT EXISTS detalle_produccion (
    id_detalle INT AUTO_INCREMENT PRIMARY KEY,
    id_produccion INT NOT NULL,
    id_producto INT NOT NULL,
    cantidad INT NOT NULL,
    CONSTRAINT fk_detalle_produccion
        FOREIGN KEY (id_produccion)
        REFERENCES produccion_diaria(id_produccion)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    CONSTRAINT fk_detalle_producto
        FOREIGN KEY (id_producto)
        REFERENCES producto(id_producto)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

CREATE INDEX idx_detalle_produccion ON detalle_produccion(id_produccion);
CREATE INDEX idx_detalle_producto ON detalle_produccion(id_producto);

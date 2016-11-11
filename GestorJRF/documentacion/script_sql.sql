CREATE DATABASE "GestorJRF"
  WITH OWNER = jrzrz
       ENCODING = 'UTF8'
       TABLESPACE = jrzrz_tablespace
       LC_COLLATE = 'Spanish_Spain.1252'
       LC_CTYPE = 'Spanish_Spain.1252'
       CONNECTION LIMIT = -1;


CREATE TABLE camion
(
  marca character varying NOT NULL,
  modelo character varying NOT NULL,
  matricula character varying NOT NULL,
  bastidor character varying NOT NULL,
  largo_caja double precision NOT NULL,
  largo_vehiculo double precision NOT NULL,
  kilometraje bigint NOT NULL,
  galibo double precision NOT NULL,
  combustible character varying NOT NULL,
  CONSTRAINT camion_pk PRIMARY KEY (bastidor),
  CONSTRAINT matricula_unique UNIQUE (matricula)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE camion
  OWNER TO jrzrz;


CREATE TABLE empleado
(
  nombre character varying NOT NULL,
  apellidos character varying NOT NULL,
  dni character varying(10) NOT NULL,
  fecha_nacimiento date NOT NULL,
  fecha_alta date NOT NULL,
  sueldo_bruto double precision NOT NULL,
  telefono character varying NOT NULL,
  email character varying NOT NULL,
  CONSTRAINT empleado_pk PRIMARY KEY (dni)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE empleado
  OWNER TO jrzrz;


CREATE TABLE empresa
(
  nombre character varying NOT NULL,
  cif character varying NOT NULL,
  domicilio character varying NOT NULL,
  localidad character varying NOT NULL,
  provincia character varying NOT NULL,
  cp integer NOT NULL,
  telefono character varying NOT NULL,
  email character varying NOT NULL,
  CONSTRAINT empresa_pk PRIMARY KEY (cif),
  CONSTRAINT nombre_unico UNIQUE (nombre)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE empresa
  OWNER TO jrzrz;


CREATE TABLE alerta
(
  id serial NOT NULL,
  bastidor character varying,
  descripcion character varying NOT NULL DEFAULT 'descripción aviso'::character varying,
  tipo_aviso character varying NOT NULL,
  CONSTRAINT aviso_pk PRIMARY KEY (id),
  CONSTRAINT aviso_fk FOREIGN KEY (bastidor)
      REFERENCES camion (bastidor) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE alerta
  OWNER TO jrzrz;


CREATE TABLE alerta_fecha
(
  dias_antelacion integer NOT NULL,
  fecha_limite date NOT NULL,
  CONSTRAINT alerta_fecha_pk PRIMARY KEY (id)
)
INHERITS (alerta)
WITH (
  OIDS=FALSE
);
ALTER TABLE alerta_fecha
  OWNER TO jrzrz;


CREATE TABLE alerta_km
(
  km_antelacion bigint NOT NULL,
  km_limite bigint NOT NULL,
  CONSTRAINT alerta_km_pk PRIMARY KEY (id)
)
INHERITS (alerta)
WITH (
  OIDS=FALSE
);
ALTER TABLE alerta_km
  OWNER TO jrzrz;


CREATE TABLE persona_contacto
(
  nombre character varying NOT NULL,
  telefono character varying NOT NULL,
  email character varying,
  cif_empresa character varying NOT NULL,
  CONSTRAINT persona_contacto_pk PRIMARY KEY (telefono, cif_empresa),
  CONSTRAINT persona_contacto_fk FOREIGN KEY (cif_empresa)
      REFERENCES empresa (cif) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);
ALTER TABLE persona_contacto
  OWNER TO jrzrz;


CREATE TABLE gasto
(
  id serial NOT NULL,
  dni character varying,
  bastidor character varying,
  concepto character varying NOT NULL,
  fecha date NOT NULL,
  descripcion character varying NOT NULL,
  precio double precision NOT NULL,
  CONSTRAINT gasto_pk PRIMARY KEY (id),
  CONSTRAINT bastidor_pk FOREIGN KEY (bastidor)
      REFERENCES camion (bastidor) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE NO ACTION,
  CONSTRAINT dni_fk FOREIGN KEY (dni)
      REFERENCES empleado (dni) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE NO ACTION
)
WITH (
  OIDS=FALSE
);
ALTER TABLE gasto
  OWNER TO jrzrz;


CREATE TABLE componente_tarifa
(
  etiqueta character varying NOT NULL,
  precio double precision NOT NULL,
  nombre_tarifa character varying NOT NULL,
  tipo_camion character varying NOT NULL,
  CONSTRAINT componente_pk PRIMARY KEY (nombre_tarifa, etiqueta, tipo_camion),
  CONSTRAINT componente_fk FOREIGN KEY (nombre_tarifa)
      REFERENCES tarifa (nombre_tarifa) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);
ALTER TABLE componente_tarifa
  OWNER TO jrzrz;


CREATE TABLE tarifa
(
  nombre_tarifa character varying NOT NULL,
  nombre_empresa character varying NOT NULL,
  CONSTRAINT tarifa_pk PRIMARY KEY (nombre_tarifa),
  CONSTRAINT tarifa_fk FOREIGN KEY (nombre_empresa)
      REFERENCES empresa (nombre) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE,
  CONSTRAINT empresa_unica UNIQUE (nombre_empresa)
)
WITH (
  OIDS=FALSE
);
ALTER TABLE tarifa
  OWNER TO jrzrz;


CREATE TABLE resumen_previo
(
  id serial NOT NULL,
  cif character varying NOT NULL,
  kilometros_ida double precision NOT NULL,
  kilometros_vuelta double precision NOT NULL,
  nombre_tarifa character varying,
  etiqueta character varying,
  tipo_camion character varying NOT NULL,
  fecha_porte date NOT NULL,
  precio_final double precision NOT NULL,
  nombre_cliente character varying NOT NULL,
  CONSTRAINT resumen_previo_pk PRIMARY KEY (id),
  CONSTRAINT cif_fk FOREIGN KEY (cif)
      REFERENCES empresa (cif) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE SET NULL,
  CONSTRAINT nombre_tarifa_fk FOREIGN KEY (nombre_tarifa)
      REFERENCES tarifa (nombre_tarifa) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE SET NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE resumen_previo
  OWNER TO jrzrz;


CREATE TABLE resumen_final
(
  id serial NOT NULL,
  referencia character varying,
  cif character varying NOT NULL,
  kilometros_ida double precision NOT NULL,
  kilometros_vuelta double precision NOT NULL,
  nombre_tarifa character varying,
  etiqueta character varying,
  tipo_camion character varying NOT NULL,
  fecha_porte date NOT NULL,
  precio_final double precision NOT NULL,
  nombre_cliente character varying NOT NULL,
  CONSTRAINT resumen_final_pk PRIMARY KEY (id),
  CONSTRAINT cif_fk FOREIGN KEY (cif)
      REFERENCES empresa (cif) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE SET NULL,
  CONSTRAINT nombre_tarifa_fk FOREIGN KEY (nombre_tarifa)
      REFERENCES tarifa (nombre_tarifa) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE SET NULL
)
WITH (
  OIDS=FALSE
);
ALTER TABLE resumen_final
  OWNER TO jrzrz;


CREATE TABLE itinerario_previo
(
  id serial NOT NULL,
  punto character varying NOT NULL,
  direccion character varying NOT NULL,
  id_resumen_previo bigint NOT NULL,
  es_etapa boolean NOT NULL,
  CONSTRAINT itinerario_previo_pk PRIMARY KEY (id),
  CONSTRAINT id_resumen_fk FOREIGN KEY (id_resumen_previo)
      REFERENCES resumen_previo (id) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);
ALTER TABLE itinerario_previo
  OWNER TO jrzrz;


CREATE TABLE itinerario_final
(
  id serial NOT NULL,
  punto character varying NOT NULL,
  direccion character varying NOT NULL,
  id_resumen_final bigint NOT NULL,
  es_etapa boolean NOT NULL,
  dni character varying NOT NULL,
  CONSTRAINT itinerario_final_pk PRIMARY KEY (id),
  CONSTRAINT dni_fk FOREIGN KEY (dni)
      REFERENCES empleado (dni) MATCH SIMPLE
      ON UPDATE NO ACTION ON DELETE SET NULL,
  CONSTRAINT id_resumen_fk FOREIGN KEY (id_resumen_final)
      REFERENCES resumen_final (id) MATCH SIMPLE
      ON UPDATE CASCADE ON DELETE CASCADE
)
WITH (
  OIDS=FALSE
);
ALTER TABLE itinerario_final
  OWNER TO jrzrz;

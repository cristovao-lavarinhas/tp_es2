BEGIN;


CREATE TABLE IF NOT EXISTS public.ativofinanceiro
(
    id serial NOT NULL,
    utilizador_id integer NOT NULL,
    data_ini date NOT NULL,
    duracao integer NOT NULL,
    imposto numeric(10, 2) NOT NULL,
    CONSTRAINT ativofinanceiro_pkey PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS public.depositoprazo
(
    id integer NOT NULL,
    valor numeric(15, 2) NOT NULL,
    banco character varying(100) COLLATE pg_catalog."default" NOT NULL,
    num_conta character varying(50) COLLATE pg_catalog."default" NOT NULL,
    titulares character varying COLLATE pg_catalog."default" NOT NULL,
    taxa_juros_anual numeric(5, 2) NOT NULL,
    CONSTRAINT depositoprazo_pkey PRIMARY KEY (id),
    CONSTRAINT depositoprazo_num_conta_key UNIQUE (num_conta)
    );

CREATE TABLE IF NOT EXISTS public.fundoinvestimento
(
    id integer NOT NULL,
    nome character varying(100) COLLATE pg_catalog."default" NOT NULL,
    montante_investido numeric(15, 2) NOT NULL,
    taxa_juros numeric(5, 2) NOT NULL,
    CONSTRAINT fundoinvestimento_pkey PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS public.imovelarrendado
(
    id integer NOT NULL,
    designacao character varying(100) COLLATE pg_catalog."default" NOT NULL,
    localizacao character varying(255) COLLATE pg_catalog."default" NOT NULL,
    valor_imovel numeric(15, 2) NOT NULL,
    valor_renda numeric(15, 2) NOT NULL,
    valor_mensal_condominio numeric(10, 2) NOT NULL,
    valor_anual_despesas numeric(10, 2) NOT NULL,
    CONSTRAINT imovelarrendado_pkey PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS public.relatorio
(
    id serial NOT NULL,
    utilizador_id integer NOT NULL,
    data_ini date NOT NULL,
    data_fim date NOT NULL,
    tipo_id integer NOT NULL,
    CONSTRAINT relatorio_pkey PRIMARY KEY (id)
    );

CREATE TABLE IF NOT EXISTS public.tiporelatorio
(
    id serial NOT NULL,
    descricao character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT tiporelatorio_pkey PRIMARY KEY (id),
    CONSTRAINT tiporelatorio_descricao_key UNIQUE (descricao)
    );

CREATE TABLE IF NOT EXISTS public.tipoutilizador
(
    id serial NOT NULL,
    descricao character varying(50) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT tipoutilizador_pkey PRIMARY KEY (id),
    CONSTRAINT tipoutilizador_descricao_key UNIQUE (descricao)
    );

CREATE TABLE IF NOT EXISTS public.utilizador
(
    id serial NOT NULL,
    nome character varying(100) COLLATE pg_catalog."default" NOT NULL,
    email character varying(100) COLLATE pg_catalog."default" NOT NULL,
    password character varying(255) COLLATE pg_catalog."default" NOT NULL,
    imposto numeric(10, 2),
    nif character varying(20) COLLATE pg_catalog."default" NOT NULL,
    tipo_id integer NOT NULL,
    CONSTRAINT utilizador_pkey PRIMARY KEY (id),
    CONSTRAINT utilizador_email_key UNIQUE (email),
    CONSTRAINT utilizador_nif_key UNIQUE (nif)
    );

ALTER TABLE IF EXISTS public.ativofinanceiro
    ADD CONSTRAINT ativofinanceiro_utilizador_id_fkey FOREIGN KEY (utilizador_id)
    REFERENCES public.utilizador (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE CASCADE;


ALTER TABLE IF EXISTS public.depositoprazo
    ADD CONSTRAINT depositoprazo_id_fkey FOREIGN KEY (id)
    REFERENCES public.ativofinanceiro (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE CASCADE;
CREATE INDEX IF NOT EXISTS depositoprazo_pkey
    ON public.depositoprazo(id);


ALTER TABLE IF EXISTS public.fundoinvestimento
    ADD CONSTRAINT fundoinvestimento_id_fkey FOREIGN KEY (id)
    REFERENCES public.ativofinanceiro (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE CASCADE;
CREATE INDEX IF NOT EXISTS fundoinvestimento_pkey
    ON public.fundoinvestimento(id);


ALTER TABLE IF EXISTS public.imovelarrendado
    ADD CONSTRAINT imovelarrendado_id_fkey FOREIGN KEY (id)
    REFERENCES public.ativofinanceiro (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE CASCADE;
CREATE INDEX IF NOT EXISTS imovelarrendado_pkey
    ON public.imovelarrendado(id);


ALTER TABLE IF EXISTS public.relatorio
    ADD CONSTRAINT relatorio_tipo_id_fkey FOREIGN KEY (tipo_id)
    REFERENCES public.tiporelatorio (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public.relatorio
    ADD CONSTRAINT relatorio_utilizador_id_fkey FOREIGN KEY (utilizador_id)
    REFERENCES public.utilizador (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE CASCADE;


ALTER TABLE IF EXISTS public.utilizador
    ADD CONSTRAINT utilizador_tipo_id_fkey FOREIGN KEY (tipo_id)
    REFERENCES public.tipoutilizador (id) MATCH SIMPLE
    ON UPDATE NO ACTION
       ON DELETE NO ACTION;

END;
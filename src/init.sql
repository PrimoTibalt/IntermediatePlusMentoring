-- This script was generated by the ERD tool in pgAdmin 4.
-- Please log an issue at https://github.com/pgadmin-org/pgadmin4/issues/new/choose if you find any bugs, including reproduction steps.
BEGIN;

CREATE TABLE IF NOT EXISTS public."Notifications"
(
    "Id" uuid NOT NULL,
    "Status" int NOT NULL,
    "Data" bytea,
    "Timestamp" timestamp NOT NULL
);

CREATE TABLE IF NOT EXISTS public."Events"
(
    "Id" serial NOT NULL,
    "VenueId" integer NOT NULL,
    "Name" text NOT NULL,
    "Description" text,
    "StartDate" date NOT NULL,
    "EndDate" date NOT NULL,
    CONSTRAINT "Events_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Venues"
(
    "Id" serial NOT NULL,
    "Name" text NOT NULL,
    "Description" text,
    "Address" text,
    CONSTRAINT "Venues_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Sections"
(
    "Id" serial NOT NULL,
    "VenueId" integer NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "Sections_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Sections_VenueId_Name_UNIQUE" UNIQUE ("VenueId", "Name")
);

CREATE TABLE IF NOT EXISTS public."Rows"
(
    "Id" serial NOT NULL,
    "SectionId" integer NOT NULL,
    "Number" smallint NOT NULL,
    CONSTRAINT "Rows_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Rows_SectionId_Number_UNIQUE" UNIQUE ("SectionId", "Number")
);

CREATE TABLE IF NOT EXISTS public."Seats"
(
    "Id" serial NOT NULL,
    "RowId" integer,
    "Number" integer NOT NULL,
    CONSTRAINT "Seats_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Seats_Number_RowId_UNIQUE" UNIQUE ("Number", "RowId")
);

CREATE TABLE IF NOT EXISTS public."EventSeats"
(
    "Id" bigserial NOT NULL,
    "EventId" integer NOT NULL,
    "SeatId" integer NOT NULL,
    "PriceId" integer NOT NULL,
    "Status" int NOT NULL,
    "Version" smallint NOT NULL DEFAULT 0,
    CONSTRAINT "EventSeats_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "EventSeats_EventId_SeatId_UNIQUE" UNIQUE ("EventId", "SeatId")
);

CREATE TABLE IF NOT EXISTS public."Prices"
(
    "Id" serial NOT NULL,
    "Type" text NOT NULL,
    "Price" money NOT NULL,
    CONSTRAINT "Prices_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."CartItems"
(
    "Id" bigserial NOT NULL,
    "CartId" uuid NOT NULL,
    "EventSeatId" bigint NOT NULL,
    "PriceId" integer NOT NULL,
    CONSTRAINT "CartItems_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "CartItems_CartId_EventSeatId_UNIQUE" UNIQUE ("CartId", "EventSeatId")
);

CREATE TABLE IF NOT EXISTS public."Carts"
(
    "Id" uuid NOT NULL,
    "UserId" integer,
    CONSTRAINT "Carts_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Users"
(
    "Id" serial NOT NULL,
    "Name" text NOT NULL,
    "Email" text NOT NULL,
    CONSTRAINT "Users_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Emain_Unique_Constraint" UNIQUE ("Email")
);

CREATE TABLE IF NOT EXISTS public."Payments"
(
    "Id" bigserial NOT NULL,
    "CartId" uuid NOT NULL,
    "Status" int NOT NULL,
    CONSTRAINT "Payments_pkey" PRIMARY KEY ("Id")
);

ALTER TABLE IF EXISTS public."Events"
    ADD CONSTRAINT "Events_VenueId_Venues_Id_FOREIGN_KEY" FOREIGN KEY ("VenueId")
    REFERENCES public."Venues" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Sections"
    ADD CONSTRAINT "Sections_VenueId_Venues_Id" FOREIGN KEY ("VenueId")
    REFERENCES public."Venues" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Rows"
    ADD CONSTRAINT "Rows_SectionId_Sections_Id_FOREIGN_KEY" FOREIGN KEY ("SectionId")
    REFERENCES public."Sections" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Seats"
    ADD CONSTRAINT "Seats_RowId_Rows_Id_FOREIGN_KEY" FOREIGN KEY ("RowId")
    REFERENCES public."Rows" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."EventSeats"
    ADD CONSTRAINT "EventSeats_EventId_Events_Id_FOREIGN_KEY" FOREIGN KEY ("EventId")
    REFERENCES public."Events" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."EventSeats"
    ADD CONSTRAINT "EventSeats_PriceId_Prices_Id_FOREIGN_KEY" FOREIGN KEY ("PriceId")
    REFERENCES public."Prices" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."EventSeats"
    ADD CONSTRAINT "EventSeats_SeatId_Seats_Id_FOREIGN_KEY" FOREIGN KEY ("SeatId")
    REFERENCES public."Seats" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."CartItems"
    ADD CONSTRAINT "CartItems_CartId_Carts_Id_FOREIGN_KEY" FOREIGN KEY ("CartId")
    REFERENCES public."Carts" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."CartItems"
    ADD CONSTRAINT "CartItems_EventSeatId_EventSeats_Id_FOREIGN_KEY" FOREIGN KEY ("EventSeatId")
    REFERENCES public."EventSeats" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."CartItems"
    ADD CONSTRAINT "CartItems_PriceId_Prices_Id_FOREIGN_KEY" FOREIGN KEY ("PriceId")
    REFERENCES public."Prices" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Carts"
    ADD CONSTRAINT "Carts_UserId_Users_Id_FOREIGN_KEY" FOREIGN KEY ("UserId")
    REFERENCES public."Users" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;


ALTER TABLE IF EXISTS public."Payments"
    ADD CONSTRAINT "Payments_CartId_Carts_Id_FOREIGN_KEY" FOREIGN KEY ("CartId")
    REFERENCES public."Carts" ("Id") MATCH FULL
    ON UPDATE NO ACTION
    ON DELETE NO ACTION;

END;
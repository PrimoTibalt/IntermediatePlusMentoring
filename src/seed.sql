-- Seed data for Venues
INSERT INTO public."Venues" ("Name", "Description", "Address") VALUES
('Venue 1', 'Description for Venue 1', 'Address for Venue 1'),
('Venue 2', 'Description for Venue 2', 'Address for Venue 2'),
('Venue 3', 'Description for Venue 3', 'Address for Venue 3');

-- Seed data for Sections
DO $$
DECLARE
    venue_id integer;
    section_id integer;
BEGIN
    FOR venue_id IN 1..3 LOOP
        FOR section_id IN 1..5 LOOP
            INSERT INTO public."Sections" ("VenueId", "Name") VALUES (venue_id, 'Section ' || section_id);
        END LOOP;
    END LOOP;
END $$;

-- Seed data for Rows
DO $$
DECLARE
    section_id integer;
    row_number smallint;
BEGIN
    FOR section_id IN 1..15 LOOP -- 3 venues * 5 sections each = 15 sections
        FOR row_number IN 1..(3 + random() * 7)::smallint LOOP -- Random number of rows between 3 and 10
            INSERT INTO public."Rows" ("SectionId", "Number") VALUES (section_id, row_number);
        END LOOP;
    END LOOP;
END $$;

-- Seed data for Seats
DO $$
DECLARE
    row_id integer;
    seat_number integer;
BEGIN
    FOR row_id IN (SELECT "Id" FROM public."Rows") LOOP
        FOR seat_number IN 1..(1 + random() * 4)::integer LOOP -- Random number of seats between 1 and 5
            INSERT INTO public."Seats" ("RowId", "Number") VALUES (row_id, seat_number);
        END LOOP;
    END LOOP;
END $$;
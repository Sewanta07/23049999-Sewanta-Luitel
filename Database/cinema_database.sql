-- Database/cinema_database.sql
-- Oracle SQL script for Movie Ticket Booking Management System

-- Optional cleanup for repeatable runs
BEGIN EXECUTE IMMEDIATE 'DROP TABLE TICKET'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE BOOKING'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE SHOWS'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE HALL'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE THEATER'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE MOVIE'; EXCEPTION WHEN OTHERS THEN NULL; END;
/
BEGIN EXECUTE IMMEDIATE 'DROP TABLE USER_TABLE'; EXCEPTION WHEN OTHERS THEN NULL; END;
/

CREATE TABLE USER_TABLE (
    User_Id        NUMBER(10)     PRIMARY KEY,
    User_Name      VARCHAR2(100)  NOT NULL,
    User_Email     VARCHAR2(150)  NOT NULL,
    User_Address   VARCHAR2(250)  NOT NULL
);

CREATE TABLE MOVIE (
    Movie_Id            NUMBER(10)    PRIMARY KEY,
    Movie_Name          VARCHAR2(150) NOT NULL,
    Movie_Release_Date  DATE          NOT NULL
);

CREATE TABLE THEATER (
    Theater_Id     NUMBER(10)    PRIMARY KEY,
    Theater_Name   VARCHAR2(150) NOT NULL,
    Theater_City   VARCHAR2(100) NOT NULL
);

CREATE TABLE HALL (
    Hall_Id        NUMBER(10)    PRIMARY KEY,
    Theater_Id     NUMBER(10)    NOT NULL,
    Hall_Number    VARCHAR2(50)  NOT NULL,
    Hall_Capacity  NUMBER(10)    NOT NULL CHECK (Hall_Capacity > 0),
    CONSTRAINT FK_HALL_THEATER
        FOREIGN KEY (Theater_Id) REFERENCES THEATER(Theater_Id)
);

CREATE TABLE SHOWS (
    Show_Id      NUMBER(10)    PRIMARY KEY,
    Movie_Id     NUMBER(10)    NOT NULL,
    Hall_Id      NUMBER(10)    NOT NULL,
    Show_Name    VARCHAR2(150) NOT NULL,
    Show_Date    DATE          NOT NULL,
    Show_Time    VARCHAR2(20)  NOT NULL,
    CONSTRAINT FK_SHOWS_MOVIE
        FOREIGN KEY (Movie_Id) REFERENCES MOVIE(Movie_Id),
    CONSTRAINT FK_SHOWS_HALL
        FOREIGN KEY (Hall_Id) REFERENCES HALL(Hall_Id)
);

CREATE TABLE BOOKING (
    Booking_Id      NUMBER(10)    PRIMARY KEY,
    User_Id         NUMBER(10)    NOT NULL,
    Show_Id         NUMBER(10)    NOT NULL,
    Booking_Date    DATE          NOT NULL,
    Booking_Status  VARCHAR2(20)  DEFAULT 'Booked' NOT NULL,
    CONSTRAINT FK_BOOKING_USER
        FOREIGN KEY (User_Id) REFERENCES USER_TABLE(User_Id),
    CONSTRAINT FK_BOOKING_SHOW
        FOREIGN KEY (Show_Id) REFERENCES SHOWS(Show_Id)
);

CREATE TABLE TICKET (
    Ticket_Id      NUMBER(10)    PRIMARY KEY,
    Booking_Id     NUMBER(10)    NOT NULL,
    Ticket_Price   NUMBER(10,2)  NOT NULL CHECK (Ticket_Price > 0),
    Seat_Number    VARCHAR2(20)  NOT NULL,
    CONSTRAINT FK_TICKET_BOOKING
        FOREIGN KEY (Booking_Id) REFERENCES BOOKING(Booking_Id)
);

-- Helpful indexes for FK lookups
CREATE INDEX IX_HALL_THEATER_ID  ON HALL(Theater_Id);
CREATE INDEX IX_SHOWS_MOVIE_ID    ON SHOWS(Movie_Id);
CREATE INDEX IX_SHOWS_HALL_ID     ON SHOWS(Hall_Id);
CREATE INDEX IX_BOOKING_USER_ID   ON BOOKING(User_Id);
CREATE INDEX IX_BOOKING_SHOW_ID   ON BOOKING(Show_Id);
CREATE INDEX IX_TICKET_BOOKING_ID ON TICKET(Booking_Id);

-- Sample data for quick testing
INSERT INTO USER_TABLE (User_Id, User_Name, User_Email, User_Address)
VALUES (1, 'Alex Sharma', 'alex@example.com', 'Kathmandu');

INSERT INTO MOVIE (Movie_Id, Movie_Name, Movie_Release_Date)
VALUES (1, 'Sample Movie', DATE '2025-01-10');

INSERT INTO THEATER (Theater_Id, Theater_Name, Theater_City)
VALUES (1, 'City Cinema', 'Kathmandu');

INSERT INTO HALL (Hall_Id, Theater_Id, Hall_Number, Hall_Capacity)
VALUES (1, 1, 'H1', 120);

INSERT INTO SHOWS (Show_Id, Movie_Id, Hall_Id, Show_Name, Show_Date, Show_Time)
VALUES (1, 1, 1, 'Evening Show', DATE '2026-03-08', '18:30');

INSERT INTO BOOKING (Booking_Id, User_Id, Show_Id, Booking_Date, Booking_Status)
VALUES (1, 1, 1, SYSDATE, 'Booked');

INSERT INTO TICKET (Ticket_Id, Booking_Id, Ticket_Price, Seat_Number)
VALUES (1, 1, 450.00, 'A1');

COMMIT;

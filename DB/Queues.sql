CREATE TABLE Queues (
    Id SERIAL PRIMARY KEY,
    Prefix CHAR(1) NOT NULL, -- เก็บ 'A' - 'Z'
    Number INT NOT NULL,     -- เก็บ 0 - 9
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- เพิ่มข้อมูลเริ่มต้น (คิวเริ่มต้น)
INSERT INTO Queues (Prefix, Number) VALUES ('A', -1);
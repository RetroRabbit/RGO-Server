INSERT INTO "Client" (id, name)
VALUES (1, 'WesBank');
INSERT INTO "Client" (id, name)
VALUES (2, 'GBZ GasLines');
INSERT INTO "Client" (id, name)
VALUES (3, 'Africa Bank');
INSERT INTO "Client" (id, name)
VALUES (4, 'ABSA Bank');
INSERT INTO "Client" (id, name)
VALUES (5, 'ABC Enterprises');
INSERT INTO "Client" (id, name)
VALUES (6, 'Company XYZ');

INSERT INTO "EmployeeAddress" (id, city, "complexName", country, "postalCode", province, "streetNumber", "suburbOrDistrict", "unitNumber")
VALUES (1, ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ');
INSERT INTO "EmployeeAddress" (id, city, "complexName", country, "postalCode", province, "streetNumber", "suburbOrDistrict", "unitNumber")
VALUES (2, ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ');
INSERT INTO "EmployeeAddress" (id, city, "complexName", country, "postalCode", province, "streetNumber", "suburbOrDistrict", "unitNumber")
VALUES (3, ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ');
INSERT INTO "EmployeeAddress" (id, city, "complexName", country, "postalCode", province, "streetNumber", "suburbOrDistrict", "unitNumber")
VALUES (4, ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ');

INSERT INTO "EmployeeEvaluationTemplate" (id, description)
VALUES (1, 'L3 Level Up');

INSERT INTO "EmployeeEvaluationTemplateItem" (id, question, section, "templateId")
VALUES (1, 'Did you meet expectations?', 'Expectations', 1);
INSERT INTO "EmployeeEvaluationTemplateItem" (id, question, section, "templateId")
VALUES (2, 'Was this a challanging experience?', 'Expectations', 1);
INSERT INTO "EmployeeEvaluationTemplateItem" (id, question, section, "templateId")
VALUES (3, 'What is your plan?', 'Goals', 1);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (1, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'tdutoit', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930887Z', 2, NULL, '0231646', 'C', 1, 4, 'Tiny', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'du Toit', '8465468', NULL, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (2, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'amanders@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.9309Z', 1, NULL, '0231646', 'A', 1, 4, 'Alicia', 'South African', '', NULL, NULL, NULL, 1, NULL, 'fake.email@gmail.com', '', 1, 2, 1, 5, 30, 'Manders', '8465468', NULL, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate", "active","inactiveReason")
VALUES (3, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'ahermanus@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930901Z', 2, NULL, '0231646', 'M', 1, 4, 'Andrewus', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 2, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Hermanus', '8465468', NULL, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (4, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'wabbartoir@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930902Z', 2, NULL, '0231646', 'D', 1, 4, 'Werner', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 3, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Abbatoir', '8465468', NULL, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (5, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'calberts@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930904Z', 2, NULL, '0231646', 'G', 1, 4, 'Celiste', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Alberts', '8465468', NULL, NULL,True,Null);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate", "active","inactiveReason")
VALUES (6, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'lvandermerwe@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930905Z', 2, NULL, '0231646', 'K', 1, 4, 'Lourens', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'van der Merwe', '8465468', NULL, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate", "active","inactiveReason")
VALUES (7, '085456565656', NULL, 'SA', TIMESTAMPTZ '1997-12-09 22:00:00Z', FALSE, 'na', 'drichter@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930906Z', 1, NULL, '0231646', 'L', 1, 6, 'Delia', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Richter', '8465468', NULL, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (8, '085456565656', 2, 'SA', TIMESTAMPTZ '1995-12-09 22:00:00Z', TRUE, 'na', 'hpaskell@gmail.com', NULL, NULL, '8464', 2, TIMESTAMPTZ '2024-03-07 13:10:48.930907Z', 2, NULL, '0231646', 'D', 1, 3, 'Haskell', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 0, 5, 30, 'Paskell', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (9, '085456565656', 2, 'SA', TIMESTAMPTZ '1993-12-09 22:00:00Z', TRUE, 'na', 'tnoah@gmail.com', NULL, NULL, '8464', 2, TIMESTAMPTZ '2024-03-07 13:10:48.930908Z', 2, NULL, '0231646', 'C', 2, 4, 'Trevorus', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 3, 20000, 30, 'Noah', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (10, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'mwallberg@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930909Z', 1, NULL, '0231646', 'l', 2, 3, 'Markus', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 0, 1000, 30, 'Wallberg', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (11, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'kalberts', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.93091Z', 1, NULL, '0231646', 'K', 2, 3, 'Karel', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Alberts', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (12, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'jthomlison@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930911Z', 1, NULL, '0231646', 'J', 2, 3, 'Jade', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Thomlison', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (13, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'qsonico@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930912Z', 1, NULL, '0231646', 'M', 2, 3, 'Quebert', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Sonico', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (14, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'fhardy@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930914Z', 1, NULL, '0231646', 'R', 2, 3, 'Felicia', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Hardy', '8465468', 1, NULL,True,NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate","active","inactiveReason")
VALUES (15, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'rgreen@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930913Z', 1, NULL, '0231646', 'E', 2, 3, 'Rachell', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Green', '8465468', 1, NULL,True,NULL);

INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (1, 1, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (2, 2, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (3, 3, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (4, 4, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (5, 5, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (6, 6, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (7, 7, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (8, 8, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (9, 9, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (10, 10, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (11, 11, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (12, 12, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (13, 13, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (14, 14, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (15, 15, 1);

INSERT INTO "EmployeeEvaluations" (id, "employeeId", "endDate", "ownerId", "startDate", subject, "templateId")
VALUES (1, 1, NULL, 1, DATE '2024-03-07', 'Peoples'' Champion Checkin', 1);
INSERT INTO "EmployeeEvaluations" (id, "employeeId", "endDate", "ownerId", "startDate", subject, "templateId")
VALUES (2, 2, NULL, 1, DATE '2024-03-07', 'Peoples'' Champion Checkin', 1);

INSERT INTO "EmployeeEvaluationAudience" (id, "employeeEvaluationId", "employeeId")
VALUES (1, 1, 1);
INSERT INTO "EmployeeEvaluationAudience" (id, "employeeEvaluationId", "employeeId")
VALUES (2, 2, 2);
INSERT INTO "EmployeeEvaluationAudience" (id, "employeeEvaluationId", "employeeId")
VALUES (3, 1, 3);

INSERT INTO "EmployeeEvaluationRatings" (id, comment, description, "employeeEvaluationId", "employeeId", score)
VALUES (1, 'No', 'Test 1', 1, 1, 1);
INSERT INTO "EmployeeEvaluationRatings" (id, comment, description, "employeeEvaluationId", "employeeId", score)
VALUES (2, 'Yes', 'Test 2', 2, 1, 2);
INSERT INTO "EmployeeEvaluationRatings" (id, comment, description, "employeeEvaluationId", "employeeId", score)
VALUES (3, 'Maybe', 'Test 3', 1, 2, 3);

SELECT setval(
pg_get_serial_sequence('"Client"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "Client") + 1,
    nextval(pg_get_serial_sequence('"Client"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeAddress"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeAddress") + 1,
    nextval(pg_get_serial_sequence('"EmployeeAddress"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationTemplate"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationTemplate") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationTemplate"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeType"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeType") + 1,
    nextval(pg_get_serial_sequence('"EmployeeType"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"FieldCode"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "FieldCode") + 1,
    nextval(pg_get_serial_sequence('"FieldCode"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"Role"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "Role") + 1,
    nextval(pg_get_serial_sequence('"Role"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"RoleAccess"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "RoleAccess") + 1,
    nextval(pg_get_serial_sequence('"RoleAccess"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"Employee"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "Employee") + 1,
    nextval(pg_get_serial_sequence('"Employee"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationTemplateItem"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationTemplateItem") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationTemplateItem"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"FieldCodeOptions"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "FieldCodeOptions") + 1,
    nextval(pg_get_serial_sequence('"FieldCodeOptions"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"PropertyAccess"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "PropertyAccess") + 1,
    nextval(pg_get_serial_sequence('"PropertyAccess"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"RoleAccessLink"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "RoleAccessLink") + 1,
    nextval(pg_get_serial_sequence('"RoleAccessLink"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeRole"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeRole") + 1,
    nextval(pg_get_serial_sequence('"EmployeeRole"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeData"', 'id'),
GREATEST(
    (SELECT MAX("id") FROM "EmployeeData") + 1,
    nextval(pg_get_serial_sequence('"EmployeeData"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluations"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluations") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluations"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationAudience"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationAudience") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationAudience"', 'id'))),
false);
    SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationRatings"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationRatings") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationRatings"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"Client"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "Client") + 1,
    nextval(pg_get_serial_sequence('"Client"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeAddress"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeAddress") + 1,
    nextval(pg_get_serial_sequence('"EmployeeAddress"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationTemplate"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationTemplate") + 1,
    nextval(pg_get_serial_sequence('"public"."EmployeeEvaluationTemplate"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeType"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeType") + 1,
    nextval(pg_get_serial_sequence('"EmployeeType"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"FieldCode"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "FieldCode") + 1,
    nextval(pg_get_serial_sequence('"FieldCode"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"Role"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "Role") + 1,
    nextval(pg_get_serial_sequence('"Role"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"RoleAccess"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "RoleAccess") + 1,
    nextval(pg_get_serial_sequence('"RoleAccess"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"Employee"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "Employee") + 1,
    nextval(pg_get_serial_sequence('"Employee"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationTemplateItem"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationTemplateItem") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationTemplateItem"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"FieldCodeOptions"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "FieldCodeOptions") + 1,
    nextval(pg_get_serial_sequence('"FieldCodeOptions"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"PropertyAccess"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "PropertyAccess") + 1,
    nextval(pg_get_serial_sequence('"PropertyAccess"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"RoleAccessLink"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "RoleAccessLink") + 1,
    nextval(pg_get_serial_sequence('"RoleAccessLink"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeRole"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeRole") + 1,
    nextval(pg_get_serial_sequence('"EmployeeRole"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeData"', 'id'),
GREATEST(
    (SELECT MAX("id") FROM "EmployeeData") + 1,
    nextval(pg_get_serial_sequence('"EmployeeData"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluations"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluations") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluations"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationAudience"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationAudience") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationAudience"', 'id'))),
false);

 SELECT setval(
pg_get_serial_sequence('"EmployeeEvaluationRatings"', 'id'),
GREATEST(
    (SELECT MAX(id) FROM "EmployeeEvaluationRatings") + 1,
    nextval(pg_get_serial_sequence('"EmployeeEvaluationRatings"', 'id'))),
false);

UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120558Z'
WHERE id = 1;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120571Z'
WHERE id = 2;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120573Z'
WHERE id = 3;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120574Z'
WHERE id = 4;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120575Z'
WHERE id = 5;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120576Z'
WHERE id = 6;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120577Z'
WHERE id = 7;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120581Z'
WHERE id = 8;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120582Z'
WHERE id = 9;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120583Z'
WHERE id = 10;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120584Z'
WHERE id = 11;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120585Z'
WHERE id = 12;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120585Z'
WHERE id = 13;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120587Z'
WHERE id = 14;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:31:17.120586Z'
WHERE id = 15;
UPDATE "EmployeeEvaluations" SET "startDate" = DATE '2024-03-11'
WHERE id = 1;
UPDATE "EmployeeEvaluations" SET "startDate" = DATE '2024-03-11'
WHERE id = 2;

UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410462Z'
WHERE id = 1;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410476Z'
WHERE id = 2;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410477Z'
WHERE id = 3;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410478Z'
WHERE id = 4;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.41048Z'
WHERE id = 5;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410481Z'
WHERE id = 6;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410482Z'
WHERE id = 7;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410484Z'
WHERE id = 8;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410485Z'
WHERE id = 9;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410486Z'
WHERE id = 10;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410487Z'
WHERE id = 11;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410491Z'
WHERE id = 12;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410492Z'
WHERE id = 13;
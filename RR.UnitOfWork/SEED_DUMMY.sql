INSERT INTO "Client" (id, name)
VALUES (1, 'Bench');
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
INSERT INTO "Client" (id, name)
VALUES (7, 'WesBank');
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
INSERT INTO "EmployeeType" (id, name)
VALUES (1, 'Executive');
INSERT INTO "EmployeeType" (id, name)
VALUES (2, 'Developer');
INSERT INTO "EmployeeType" (id, name)
VALUES (3, 'Designer');
INSERT INTO "EmployeeType" (id, name)
VALUES (4, 'Scrum Master');
INSERT INTO "EmployeeType" (id, name)
VALUES (5, 'Business Support');
INSERT INTO "EmployeeType" (id, name)
VALUES (6, 'Account Manager');
INSERT INTO "EmployeeType" (id, name)
VALUES (7, 'People Champion');
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (1, 0, 'degree', NULL, FALSE, NULL, 'Degree', NULL, FALSE, 0, 1);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (2, 0, 'tenure', NULL, FALSE, NULL, 'Tenure', NULL, FALSE, 0, 1);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (3, 0, 'nqf', NULL, FALSE, NULL, 'NQF Level', NULL, FALSE, 0, 4);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (4, 0, 'institution', NULL, FALSE, NULL, 'Institution', NULL, FALSE, 0, 4);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (5, 0, 'experience', NULL, FALSE, NULL, 'Experience', NULL, FALSE, 0, 4);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (6, 1, 'cv', NULL, FALSE, NULL, 'CV Link', NULL, FALSE, 0, 1);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (8, 1, 'skills', NULL, FALSE, NULL, 'Tech Stack', NULL, FALSE, 0, 1);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (9, 1, 'engagement', NULL, FALSE, NULL, 'Engagement', NULL, FALSE, 0, 4);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (12, 2, 'location', NULL, FALSE, NULL, 'Location', NULL, FALSE, 0, 4);
INSERT INTO "FieldCode" (id, category, code, description, internal, "internalTable", name, regex, required, status, type)
VALUES (14, 2, 'risk', NULL, FALSE, NULL, 'Risk', NULL, FALSE, 0, 4);
INSERT INTO "Role" (id, "Description")
VALUES (1, 'SuperAdmin');
INSERT INTO "Role" (id, "Description")
VALUES (2, 'Admin');
INSERT INTO "Role" (id, "Description")
VALUES (3, 'Employee');
INSERT INTO "Role" (id, "Description")
VALUES (4, 'Talent');
INSERT INTO "Role" (id, "Description")
VALUES (5, 'Journey');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (1, 'Employee Data', 'ViewEmployee');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (2, 'Employee Data', 'AddEmployee');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (3, 'Employee Data', 'EditEmployee');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (4, 'Employee Data', 'DeleteEmployee');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (5, 'Charts', 'ViewChart');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (6, 'Charts', 'AddChart');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (7, 'Charts', 'EditChart');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (8, 'Charts', 'DeleteChart');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (9, 'Employee Data', 'ViewOwnInfo');
INSERT INTO "RoleAccess" (id, grouping, permission)
VALUES (10, 'Employee Data', 'EditOwnInfo');

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (3, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'ahermanus@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930901Z', 2, NULL, '0231646', 'M', 1, 4, 'Andrewus', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 3, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Hermanus', '8465468', NULL, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (5, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'calberts@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930904Z', 2, NULL, '0231646', 'G', 1, 4, 'Celiste', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Alberts', '8465468', NULL, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (6, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'lvandermerwe@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930905Z', 2, NULL, '0231646', 'K', 1, 4, 'Lourens', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'van der Merwe', '8465468', NULL, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (7, '085456565656', NULL, 'SA', TIMESTAMPTZ '1997-12-09 22:00:00Z', FALSE, 'na', 'drichter@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930906Z', 1, NULL, '0231646', 'L', 1, 6, 'Delia', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Richter', '8465468', NULL, NULL);

INSERT INTO "EmployeeEvaluationTemplateItem" (id, question, section, "templateId")
VALUES (1, 'Did you meet expectations?', 'Expectations', 1);
INSERT INTO "EmployeeEvaluationTemplateItem" (id, question, section, "templateId")
VALUES (2, 'Was this a challanging experience?', 'Expectations', 1);
INSERT INTO "EmployeeEvaluationTemplateItem" (id, question, section, "templateId")
VALUES (3, 'What is your plan?', 'Goals', 1);

INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (1, 14, 'Very Low');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (2, 14, 'Low');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (3, 14, 'Medium');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (4, 14, 'High');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (5, 14, 'Very High');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (6, 14, 'Unknown');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (7, 12, 'JHB');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (8, 12, 'PTA');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (9, 12, 'CP');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (10, 12, 'Other');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (11, 9, 'Very Low');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (12, 9, 'Low');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (13, 9, 'Average');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (14, 9, 'High');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (15, 9, 'Unknown');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (16, 5, 'Grad');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (17, 5, '1+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (18, 5, '2+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (19, 5, '3+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (20, 5, '4+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (21, 5, '5+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (22, 5, '6+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (23, 5, '7+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (24, 5, '8+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (25, 5, '9+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (26, 5, '10+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (27, 5, '15+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (28, 5, '20+');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (29, 4, 'TUT');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (30, 4, 'Belgium Campus');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (31, 4, 'University of Limpopo');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (32, 4, 'University of Pretoria');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (33, 4, 'NWU - Potch');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (34, 4, 'Pearson');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (35, 4, 'University of Johannesburg');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (36, 4, 'Other');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (37, 4, 'Open Window');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (38, 4, 'University of Cape Town');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (39, 4, 'Rhodes');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (40, 4, 'University of Kwa-Zulu Natal');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (41, 4, 'UNISA');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (42, 4, 'University of Witwatersrand');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (43, 3, 'NQF 4');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (44, 3, 'NQF 5');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (45, 3, 'NQF 6');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (46, 3, 'NQF 7');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (47, 3, 'NQF 8');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (48, 3, 'NQF 9');
INSERT INTO "FieldCodeOptions" (id, "fieldCodeId", option)
VALUES (49, 3, 'NQF 10');

INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (1, 1, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (2, 2, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (3, 3, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (4, 4, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (5, 5, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (6, 6, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (7, 7, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (8, 8, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (9, 9, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (10, 10, 1);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (11, 1, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (12, 2, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (13, 3, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (14, 4, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (15, 5, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (16, 6, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (17, 7, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (18, 8, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (19, 9, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (20, 10, 2);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (21, 1, 3);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (22, 3, 3);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (23, 9, 3);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (24, 10, 3);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (25, 5, 4);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (26, 6, 4);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (27, 7, 4);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (28, 8, 4);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (29, 9, 4);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (30, 10, 4);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (31, 5, 5);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (32, 6, 5);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (33, 7, 5);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (34, 8, 5);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (35, 9, 5);
INSERT INTO "RoleAccessLink" (id, "roleAccessId", "roleId")
VALUES (36, 10, 5);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (1, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'tdutoit', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930887Z', 2, NULL, '0231646', 'C', 1, 4, 'Tiny', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 3, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'du Toit', '8465468', NULL, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (2, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'amanders@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.9309Z', 2, NULL, '0231646', 'A', 1, 4, 'Alicia', 'South African', '', NULL, NULL, NULL, 1, 3, 'fake.email@gmail.com', '', 1, 2, 1, 5, 30, 'Manders', '8465468', NULL, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (4, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', TRUE, 'na', 'wabbartoir@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930902Z', 2, NULL, '0231646', 'D', 1, 4, 'Werner', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 3, 'test@gmail.com', '', 1, 2, 1, 5, 30, 'Abbatoir', '8465468', NULL, NULL);

INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (3, 3, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (5, 5, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (6, 6, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (7, 7, 1);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (8, '085456565656', 2, 'SA', TIMESTAMPTZ '1995-12-09 22:00:00Z', TRUE, 'na', 'hpaskell@gmail.com', NULL, NULL, '8464', 2, TIMESTAMPTZ '2024-03-07 13:10:48.930907Z', 2, NULL, '0231646', 'D', 1, 3, 'Haskell', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 0, 5, 30, 'Paskell', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (9, '085456565656', 2, 'SA', TIMESTAMPTZ '1993-12-09 22:00:00Z', TRUE, 'na', 'tnoah@gmail.com', NULL, NULL, '8464', 2, TIMESTAMPTZ '2024-03-07 13:10:48.930908Z', 2, NULL, '0231646', 'C', 2, 4, 'Trevorus', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, NULL, 'test@gmail.com', '', 1, 2, 3, 20000, 30, 'Noah', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (10, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'mwallberg@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930909Z', 1, NULL, '0231646', 'l', 2, 3, 'Markus', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 0, 1000, 30, 'Wallberg', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (11, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'kalberts', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.93091Z', 1, NULL, '0231646', 'K', 2, 3, 'Karel', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Alberts', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (12, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'jthomlison@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930911Z', 1, NULL, '0231646', 'J', 2, 3, 'Jade', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Thomlison', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (13, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'qsonico@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930912Z', 1, NULL, '0231646', 'M', 2, 3, 'Quebert', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Sonico', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (14, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'fhardy@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930914Z', 1, NULL, '0231646', 'R', 2, 3, 'Felicia', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Hardy', '8465468', 1, NULL);

INSERT INTO "Employee" (id, "cellphoneNo", "clientAllocated", "countryOfBirth", "dateOfBirth", disability, "disabilityNotes", email, "emergencyContactName", "emergencyContactNo", "employeeNumber", "employeeTypeId", "engagementDate", gender, "houseNo", "idNumber", initials, "leaveInterval", level, name, nationality, notes, "passportCountryIssue", "passportExpirationDate", "passportNumber", "payRate", "peopleChampion", "personalEmail", photo, "physicalAddress", "postalAddress", race, salary, "salaryDays", surname, "taxNumber", "teamLead", "terminationDate")
VALUES (15, '085456565656', NULL, 'SA', TIMESTAMPTZ '1996-12-09 22:00:00Z', FALSE, 'na', 'rgreen@gmail.com', NULL, NULL, '8464', 7, TIMESTAMPTZ '2024-03-07 13:10:48.930913Z', 1, NULL, '0231646', 'E', 2, 3, 'Rachell', 'South African', 'Cannot English very good', NULL, NULL, NULL, 1, 4, 'test@gmail.com', '', 1, 2, 1, 1000, 30, 'Green', '8465468', 1, NULL);

INSERT INTO "EmployeeData" ("id", "employeeId", "fieldCodeId", value)
VALUES (1, 1, 14, 'John');

INSERT INTO "EmployeeEvaluations" (id, "employeeId", "endDate", "ownerId", "startDate", subject, "templateId")
VALUES (1, 1, NULL, 1, DATE '2024-03-07', 'Peoples'' Champion Checkin', 1);
INSERT INTO "EmployeeEvaluations" (id, "employeeId", "endDate", "ownerId", "startDate", subject, "templateId")
VALUES (2, 2, NULL, 1, DATE '2024-03-07', 'Peoples'' Champion Checkin', 1);

INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (1, 1, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (2, 2, 1);
INSERT INTO "EmployeeRole" (id, "employeeId", "roleId")
VALUES (4, 4, 1);

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
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410494Z'
WHERE id = 14;
UPDATE "Employee" SET "engagementDate" = TIMESTAMPTZ '2024-03-11 06:48:18.410493Z'
WHERE id = 15;

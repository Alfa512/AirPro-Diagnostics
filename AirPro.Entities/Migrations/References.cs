using AirPro.Common.Enumerations;
using AirPro.Entities.Access;
using AirPro.Entities.Billing;
using AirPro.Entities.Common;
using AirPro.Entities.Diagnostics;
using AirPro.Entities.Notifications;
using AirPro.Entities.Repair;
using AirPro.Entities.Scan;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UniMatrix.Common.Extensions;

namespace AirPro.Entities.Migrations
{
    public sealed class References
    {
        public static void SeedAccess(ref EntityDbContext context)
        {
            // Build Roles from Enum.
            var roles = (from int role in Enum.GetValues(typeof(ApplicationRoles))
                select new RoleEntityModel
                {
                    Id = ((ApplicationRoles) role).GetEnumGuid(),
                    Name = Enum.GetName(typeof(ApplicationRoles), role)
                }).ToList();

            // Populate Security Info.
            context.Roles.AddOrUpdate(r => r.Id, roles.ToArray<RoleEntityModel>());

            // Remove Unused Roles.
            var current = roles.Select(r => r.Id).ToList();
            foreach (var role in context?.Roles?.Where(r => !current.Contains(r.Id)).ToList() ?? new List<RoleEntityModel>())
            {
                // Remove Role.
                context?.Roles?.Remove(role);
            }
        }

        public static void SeedPointOfImpacts(ref EntityDbContext context)
        {
            // Build Types from Enum.
            var types = (from int type in Enum.GetValues(typeof(PointOfImacts))
                select new PointOfImpactEntityModel
                {
                    PointOfImpactId = type,
                    Name = Enum.GetName(typeof(PointOfImacts), type)
                }).ToList();

            var totalPois = context.PointOfImpacts.Count();

            // Populate Types.
            context.PointOfImpacts.AddOrUpdate(t => t.PointOfImpactId, types.ToArray<PointOfImpactEntityModel>());

            if (totalPois != 6) return;
            context.SaveChanges();

            context.Database.ExecuteSqlCommand(
                "UPDATE Repair.OrderPointOfImpacts SET PointOfImpactId = '11' WHERE PointOfImpactId = @p1",
                new SqlParameter("@p1", 1));
            context.Database.ExecuteSqlCommand(
                "UPDATE Repair.OrderPointOfImpacts SET PointOfImpactId = '1' WHERE PointOfImpactId = @p1",
                new SqlParameter("@p1", 2));
            context.Database.ExecuteSqlCommand(
                "UPDATE Repair.OrderPointOfImpacts SET PointOfImpactId = '9' WHERE PointOfImpactId = @p1",
                new SqlParameter("@p1", 3));
            context.Database.ExecuteSqlCommand(
                "UPDATE Repair.OrderPointOfImpacts SET PointOfImpactId = '3' WHERE PointOfImpactId = @p1",
                new SqlParameter("@p1", 4));
            context.Database.ExecuteSqlCommand(
                "UPDATE Repair.OrderPointOfImpacts SET PointOfImpactId = '8' WHERE PointOfImpactId = @p1",
                new SqlParameter("@p1", 5));
            context.Database.ExecuteSqlCommand(
                "UPDATE Repair.OrderPointOfImpacts SET PointOfImpactId = '4' WHERE PointOfImpactId = @p1",
                new SqlParameter("@p1", 6));
        }

        public static void SeedNotificationTypes(ref EntityDbContext context)
        {
            // Build Types from Enum.
            var types = (from int type in Enum.GetValues(typeof(NotificationTypes))
                         select new NotificationTypeEntityModel()
                         {
                             TypeGuid = ((NotificationTypes)type).GetEnumGuid(),
                             Name = Enum.GetName(typeof(NotificationTypes), type)
                         }).ToList();

            // Populate Types.
            context.NotificationTypes.AddOrUpdate(t => t.TypeGuid, types.ToArray<NotificationTypeEntityModel>());

            // TODO: Remove Unused Types?
        }

        public static void SeedDiagnosticTools(ref EntityDbContext context)
        {
            // Build Tools from Enum.
            var tools = (from int tool in Enum.GetValues(typeof(DiagnosticTool))
                select new DiagnosticToolEntityModel()
                {
                    DiagnosticToolId = tool,
                    DiagnosticToolName = Enum.GetName(typeof(DiagnosticTool), tool)
                }).ToList();

            // Find Updates.
            var updates = tools.Except(context.DiagnosticTools.ToList());

            // Populate Tools.
            context.DiagnosticTools.AddOrUpdate(t => t.DiagnosticToolId, updates.ToArray());
        }

        public static void SeedDiagnosticUploadFileTypes(ref EntityDbContext context)
        {
            // Build Tools from Enum.
            var types = (from int type in Enum.GetValues(typeof(DiagnosticFileType))
                select new DiagnosticUploadFileTypeEntityModel()
                {
                    UploadFileTypeId = type,
                    UploadFileTypeName = Enum.GetName(typeof(DiagnosticFileType), type)
                }).ToList();

            // Find Updates.
            var updates = types.Except(context.DiagnosticUploadFileTypes.ToList());

            // Populate Tools.
            context.DiagnosticUploadFileTypes.AddOrUpdate(u => u.UploadFileTypeId, updates.ToArray());
        }

        public static void SeedUploadTypes(ref EntityDbContext context)
        {
            // Build Types from Enum.
            var types = (from int type in Enum.GetValues(typeof(UploadType))
                select new UploadTypeEntityModel()
                {
                    UploadTypeId = type,
                    UploadTypeName = Enum.GetName(typeof(UploadType), type)
                }).ToList();

            // Find Updates.
            var updates = types.Except(context.UploadTypes.ToList());

            // Populate Types.
            context.UploadTypes.AddOrUpdate(u => u.UploadTypeId, updates.ToArray());
        }

        public static void SeedRepairStatuses(ref EntityDbContext context)
        {
            // Build Types from Enum.
            var types = (from int type in Enum.GetValues(typeof(RepairStatuses))
                select new OrderStatusEntityModel()
                {
                    StatusId = type,
                    StatusName = Enum.GetName(typeof(RepairStatuses), type)
                }).ToList();

            // Find Updates.
            var updates = types.Except(context.RepairOrderStatuses.ToList());

            // Populate Types.
            context.RepairOrderStatuses.AddOrUpdate(s => s.StatusId, updates.ToArray());
        }

        public static void SeedNoteTypes(ref EntityDbContext context)
        {
            // Build Types from Enum.
            var types = (from int type in Enum.GetValues(typeof(NoteType))
                select new NoteTypeEntityModel()
                {
                    NoteTypeId = type,
                    NoteTypeName = Enum.GetName(typeof(NoteType), type)
                }).ToList();

            // Find Updates.
            var updates = types.Except(context.NoteTypes.ToList());

            // Populate Types.
            context.NoteTypes.AddOrUpdate(u => u.NoteTypeId, updates.ToArray());
        }

        public static void SeedDefaults(ref EntityDbContext context)
        {
            // Update Default System User.
            context.Users.AddOrUpdate(
                u => u.UserName,
                new UserEntityModel
                {
                    UserName = "system@airprodiag.com",
                    Email = "system@airprodiag.com",
                    FirstName = "AirPro",
                    LastName = "System",
                    JobTitle = "System Account",
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    LockoutEnabled = true,
                    LockoutEndDateUtc = DateTime.MaxValue
                });

            context.SaveChanges();

            SetPaymentTypes(context);

            SetRequestCategories(context);

            SetCurrencies(context);

            SetNotificationTemplates(context);
        }

        private static void SetNotificationTemplates(EntityDbContext context)
        {
            // Populate Default Notification Templates.
            if (context.NotificationTemplates.Any()) return;

            // Find Default User.
            var user = context.Users.Find(Guid.Empty);

            // Billing Notification Template
            context.NotificationTemplates.Add(
                new NotificationTemplateEntityModel
                {
                    Name = "BillingEmail",
                    Options =
                        "InvoiceID,InvoiceTotal,RepairID,VehicleVIN,VehicleMake,VehicleModel,VehicleYear,ShopRONumber,ShopName,ShopPhone",
                    Subject = "New Invoice (ID {InvoiceID})",
                    EmailBody = "<p>Invoice ID {InvoiceID} totaling {InvoiceTotal} is ready to be viewed.</p>",
                    CreatedBy = user,
                    CreatedDt = DateTimeOffset.UtcNow
                }
            );

            // Shop Report Notification Template
            context.NotificationTemplates.Add(
                new NotificationTemplateEntityModel
                {
                    Name = "ShopReportEmail",
                    Options =
                        "RequestID,RequestType,ProblemDesc,RepairID,VehicleVIN,VehicleMake,VehicleModel,VehicleYear,ShopRONumber,ShopName",
                    Subject = "Scan Report Completed (Request ID {RequestID})",
                    EmailBody = "<p>Scan Report ID {RequestID} is ready to be viewed.</p>",
                    CreatedBy = user,
                    CreatedDt = DateTimeOffset.UtcNow
                }
            );

            // Shop Invoice Notifciation Template
            context.NotificationTemplates.Add(
                new NotificationTemplateEntityModel
                {
                    Name = "ShopInvoiceEmail",
                    Options = "InvoiceID,RepairID,VehicleVIN,VehicleMake,VehicleModel,VehicleYear,ShopRONumber,ShopName",
                    Subject = "New Invoice (ID {InvoiceID})",
                    EmailBody = "<p>Invoice ID {InvoiceID} is ready to be viewed.</p>",
                    CreatedBy = user,
                    CreatedDt = DateTimeOffset.UtcNow
                }
            );

            // Scan Request Notification Template
            context.NotificationTemplates.Add(
                new NotificationTemplateEntityModel
                {
                    Name = "ScanRequestEmail",
                    Options =
                        "RequestID,RequestLink,RequestType,ProblemDesc,RepairID,VehicleVIN,VehicleMake,VehicleModel,VehicleYear,ShopRONumber,ShopName,ShopPhone",
                    Subject = "New Scan Request (ID {RequestID})",
                    EmailBody = "<p>Scan Request ID {RequestID} is ready to be viewed.  {RequestLink}</p>",
                    CreatedBy = user,
                    CreatedDt = DateTimeOffset.UtcNow
                }
            );

            // Statement Notification Template
            context.NotificationTemplates.Add(
                new NotificationTemplateEntityModel
                {
                    Name = "ShopStatementEmail",
                    Options =
                        "PaymentID,ShopName,StatementLink,PaymentAmount,PaymentReferenceNumber,PaymentMemo,PaymentType,PaymentCurrency,DiscountPercentage",
                    Subject = "New Payment Statement (ID {PaymentId})",
                    EmailBody = "<p>Payment ID {PaymentID} is ready to be viewed.</p>",
                    CreatedBy = user,
                    CreatedDt = DateTimeOffset.UtcNow
                }
            );
        }

        private static void SetPaymentTypes(EntityDbContext context)
        {
            var paymentTypes = new[]
            {
                new PaymentTypeEntityModel
                {
                    PaymentTypeName = "Admin",
                    PaymentTypeSortOrder = 1,
                    PaymentTypeActiveInd = true
                },
                new PaymentTypeEntityModel
                {
                    PaymentTypeName = "Cash",
                    PaymentTypeSortOrder = 2,
                    PaymentTypeActiveInd = true
                },
                new PaymentTypeEntityModel
                {
                    PaymentTypeName = "Check",
                    PaymentTypeSortOrder = 3,
                    PaymentTypeActiveInd = true
                },
                new PaymentTypeEntityModel
                {
                    PaymentTypeName = "Charge",
                    PaymentTypeSortOrder = 4,
                    PaymentTypeActiveInd = true
                },
                new PaymentTypeEntityModel
                {
                    PaymentTypeName = "ACH/EFT",
                    PaymentTypeSortOrder = 5,
                    PaymentTypeActiveInd = true
                }
            };

            // Populate Payment Types.
            if (!context.PaymentTypes.Any())
            {
                context.PaymentTypes.AddOrUpdate(
                    t => t.PaymentTypeName, paymentTypes);
            }
            else
            {
                //Make sure we have the ones in the array
                var dbTypes = context.PaymentTypes.ToList();
                var newToAdd = paymentTypes
                    .GroupJoin(dbTypes, db => db.PaymentTypeName, arr => arr.PaymentTypeName,
                        (arr, db) => new {Local = arr, Db = db.SingleOrDefault()}).Where(t => t.Db == null)
                    .Select(d => d.Local).ToList();

                context.PaymentTypes.AddRange(newToAdd);
            }
        }

        private static void SetRequestCategories(EntityDbContext context)
        {
            var requestTypeOptions = new[]
            {
                new RequestCategoryEntityModel
                {
                    RequestCategoryId = 1,
                    RequestCategoryName = "Pre-Scan",
                    Order = 1,
                    IsActive = true
                },
                new RequestCategoryEntityModel
                {
                    RequestCategoryId = 2,
                    RequestCategoryName = "Post-Scan",
                    Order = 2,
                    IsActive = true
                }
            };

            // Populate RequestTypeOptionsCatalog Types.
            if (!context.ScanRequestCategories.Any())
            {
                context.ScanRequestCategories.AddOrUpdate(
                    t => t.RequestCategoryName, requestTypeOptions);
            }
            else
            {
                //Make sure we have the ones in the array
                var dbTypes = context.ScanRequestCategories.ToList();
                var newToAdd = requestTypeOptions
                    .GroupJoin(dbTypes, db => db.RequestCategoryName, arr => arr.RequestCategoryName,
                        (arr, db) => new {Local = arr, Db = db.SingleOrDefault()}).Where(t => t.Db == null)
                    .Select(d => d.Local).ToList();

                context.ScanRequestCategories.AddRange(newToAdd);
            }
        }

        private static void SetCurrencies(EntityDbContext context)
        {
            var currencies = new[]
            {
                new CurrencyEntityModel
                {
                    CurrencyId = 1,
                    Name = "USD"
                },
                new CurrencyEntityModel
                {
                    CurrencyId = 2,
                    Name = "CAD"
                }
            };

            if (!context.Currencies.Any())
            {
                context.Currencies.AddOrUpdate(t => t.Name, currencies);

                context.SaveChanges();

                //Set Pricing Plans
                context.Database.ExecuteSqlCommand(
                    "UPDATE Billing.PricingPlans SET CurrencyId = 1 WHERE CurrencyId IS NULL");

                //Set Invoices
                context.Database.ExecuteSqlCommand(
                    "UPDATE Repair.Invoices SET CurrencyId = 1 WHERE CurrencyId IS NULL");

                //Set Payments
                context.Database.ExecuteSqlCommand(
                    "UPDATE Billing.Payments SET CurrencyId = 1 WHERE CurrencyId IS NULL");
            }
            else
            {
                //Make sure we have the ones in the array
                var dbTypes = context.Currencies.ToList();
                var newToAdd = currencies
                    .GroupJoin(dbTypes, db => db.Name, arr => arr.Name,
                        (arr, db) => new {Local = arr, Db = db.SingleOrDefault()}).Where(t => t.Db == null)
                    .Select(d => d.Local).ToList();

                context.Currencies.AddRange(newToAdd);
            }
        }

        public static void SeedLookups(ref EntityDbContext context)
        {
            // Find Default User.
            var user = context.Users.Find(Guid.Empty);

            // Populate Scan Request Types.
            /*
            public enum RequestScanType
            {
                QuickScan = 1,
                Diagnostic = 2,
                Completion = 3,
                FollowUp = 4
            }
            */
            if (!context.ScanRequestTypes.Any())
                context.ScanRequestTypes.AddOrUpdate(
                    t => t.RequestTypeId,
                    new Scan.RequestTypeEntityModel { RequestTypeId = 1, TypeName = "Quick Scan", SortOrder = 1, ActiveFlag = true, BillableFlag = false, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 2, TypeName = "Diagnostic Scan", SortOrder = 3, ActiveFlag = true, BillableFlag = true, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 3, TypeName = "Completion Scan", SortOrder = 4, ActiveFlag = true, BillableFlag = true, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 4, TypeName = "Follow Up Scan", SortOrder = 5, ActiveFlag = true, BillableFlag = true, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 5, TypeName = "Inspection Scan", SortOrder = 2, ActiveFlag = true, BillableFlag = true, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 6, TypeName = "Self Scan", SortOrder = 6, ActiveFlag = false, BillableFlag = false, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 7, TypeName = "Scan Analysis", SortOrder = 7, ActiveFlag = true, BillableFlag = true, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow },
                    new Scan.RequestTypeEntityModel { RequestTypeId = 8, TypeName = "Demo Scan", SortOrder = 8, ActiveFlag = true, BillableFlag = true, CreatedBy = user, CreatedDt = DateTimeOffset.UtcNow }
                    );

            // Populate Warning Indicators.
            /*
            public enum WarningIndicator
            {
                CheckEngine = 1,
                ABS = 2,
                Airbag = 3,
                TPMS = 4,
                Stability = 5,
                Security = 6
            }
            */
            if (!context.ScanRequestWarningIndicators.Any())
                context.ScanWarningIndicators.AddOrUpdate(
                    i => i.WarningIndicatorId,
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 1, Name = "Check Engine" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 2, Name = "ABS" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 3, Name = "Airbag" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 4, Name = "TPMS" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 5, Name = "Stability" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 6, Name = "Security" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 7, Name = "Other" },
                    new Scan.WarningIndicatorEntityModel { WarningIndicatorId = 8, Name = "None" }
                    );
        }

        public static void CreateViews(ref EntityDbContext context)
        {
            var path = $"{Assembly.GetExecutingAssembly().GetExecutingAssemblyPath()}/Views";
            var query = "SELECT TABLE_SCHEMA + '.' + TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS";

            ProcessSqlFiles(ref context, path, query);
        }

        public static void SeedUserDefinedTypes(ref EntityDbContext context)
        {
            var path = $"{Assembly.GetExecutingAssembly().GetExecutingAssemblyPath()}/UserDefinedTypes";
            var query = "SELECT s.name + '.' + tt.name FROM sys.table_types tt INNER JOIN sys.schemas s ON tt.schema_id = s.schema_id";

            ProcessSqlFiles(ref context, path, query);
        }

        public static void CreateTriggers(ref EntityDbContext context)
        {
            var path = $"{Assembly.GetExecutingAssembly().GetExecutingAssemblyPath()}/Triggers";
            var query = "SELECT S.name + '.' + t.name + '.' + tr.name FROM sys.triggers tr " +
                        "INNER JOIN sys.tables t ON t.object_id = tr.parent_id " +
                        "INNER JOIN sys.schemas s ON t.schema_id = s.schema_id";

            ProcessSqlFiles(ref context, path, query);
        }

        public static void CreateProcedures(ref EntityDbContext context)
        {
            var path = $"{Assembly.GetExecutingAssembly().GetExecutingAssemblyPath()}/Procedures";
            var query = "SELECT ROUTINE_SCHEMA + '.' + ROUTINE_NAME FROM INFORMATION_SCHEMA.ROUTINES";

            ProcessSqlFiles(ref context, path, query);
        }

        private static void ProcessSqlFiles(ref EntityDbContext context, string path, string query)
        {
            var files = Directory.GetFiles(path).Select(Path.GetFileNameWithoutExtension).ToList();
            var objects = context.Database.SqlQuery<string>(query).ToList();
            var updates = files.Except(objects);

            foreach (var update in updates)
            {
                foreach (var statement in GetSqlStatements($"{path}/{update}.sql"))
                {
                    if (!string.IsNullOrWhiteSpace(statement))
                    {
                        if (statement.Contains("FULLTEXT"))
                        {
                            using (var conn = new SqlConnection(context.Database.Connection.ConnectionString))
                            {
                                conn.Open();
                                using (var cmd = new SqlCommand(statement, conn))
                                {
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        else
                        {
                            context.Database.ExecuteSqlCommand(statement);
                        }
                    }
                }
            }
        }

        private static IEnumerable<string> GetSqlStatements(string file)
        {
            var result = new List<string>();
            var statement = new StringBuilder();
            foreach (var line in File.ReadAllLines(file))
            {
                // Check for GO.
                if (line.Trim().ToUpper() == "GO")
                {
                    // Complete Statement.
                    result.Add(statement.ToString());
                    statement.Clear();
                }
                else
                {
                    // Add Line.
                    statement.AppendLine(line);
                }
            }

            // Add Statement to Result.
            if (statement.Length > 0)
                result.Add(statement.ToString());

            return result;
        }
    }
}
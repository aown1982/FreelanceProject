using DAL;
using DAL.UserData;
using RESTfulBAL.Models.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RESTfulBAL.Controllers.CustomUserData
{
    public class C_UserDataController : ApiController
    {
        private UserDataEntities db = new UserDataEntities();

        // GET: api/GetUserDerivedNoteByCategory/123/Allergies
        [Route("api/CustomUserData/GetUserDataByCategoryWithNotes/{userId}/{category}")]
        public List<Models.UserData.UserDataWithNote> GetUserDataByCategoryWithNotes(int userId, string category)
        {
            // System status Id == 1 - Valid Entry
            List<Models.UserData.UserDataWithNote> retVal = new List<Models.UserData.UserDataWithNote>();
            switch (category)
            {
                case "Allergies":
                    #region Allergies
                    List<tUserAllergy> allUserAllergies = db.tUserAllergies
                                                                .Include("tAllergen")
                                                                .Include("tReaction")
                                                                .Include("tAllergySeverity")
                                                                .Include("tAllergyStatus")
                                                                .Include("tUserSourceService.tSourceService")
                                                                .Include("tSourceOrganization.tOrganization")
                                                                .Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserAllergy item in allUserAllergies)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Allergen = item.tAllergen.Name,
                            Reaction = item.tReaction != null ? item.tReaction.Name : string.Empty,
                            Severity = item.tAllergySeverity != null ? item.tAllergySeverity.Severity : string.Empty,
                            Status = item.tAllergyStatus.Status,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Care Plans":
                    #region Care Plans
                    List<tUserCarePlan> allUserCarePlans = db.tUserCarePlans.Include("tUserCarePlanSpecialty").Include("tUserCarePlanType").Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserCarePlan item in allUserCarePlans)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            CarePlanSpecialtyName = item.tUserCarePlanSpecialty != null ? item.tUserCarePlanSpecialty.Name : "",
                            CarePlanTypeName = item.tUserCarePlanType != null ? item.tUserCarePlanType.Name : "",
                            CarePlanText = item.Text,
                            CarePlanName = item.Name,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Encounters":
                    #region Encounters
                    List<tUserEncounter> allUserEncounter = db.tUserEncounters.Include("tProvider").Include("tVisitType").Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserEncounter item in allUserEncounter)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            EncounterDateTime = item.EncounterDateTime,
                            ProviderName = item.ProviderID != null ? item.tProvider.Name : "",
                            VisitTypeName = item.VisitTypeID != null ? item.tVisitType.Name : "",
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Functional Statuses":
                    #region Functional Statuses
                    List<tUserFunctionalStatus> allUserFunctionalStatus = db.tUserFunctionalStatuses.Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserFunctionalStatus item in allUserFunctionalStatus)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Name = item.Name,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Immunizations":
                    #region Immunizations
                    List<tUserImmunization> allUserImmunization = db.tUserImmunizations.Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserImmunization item in allUserImmunization)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            Name = item.Name,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Instructions":
                    #region Instructions
                    List<tUserInstruction> allUserInstructions = db.tUserInstructions.Include("tInstructionType").Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserInstruction item in allUserInstructions)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            Name = item.Name,
                            Text = item.Text,
                            InstructionTypeName = item.InstructionTypeID != null ? item.tInstructionType.Name : "",
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;

                case "Narratives":
                    #region Narratives
                    List<tUserNarrative> allUserNarratives = db.tUserNarratives.Include("tProvider").Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserNarrative item in allUserNarratives)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            ProviderName = item.tProvider != null ? item.tProvider.Name : string.Empty,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Prescriptions":
                    #region Prescriptions
                    List<tUserPrescription> allUserPrescriptions = db.tUserPrescriptions.Include("tProvider")
                                            .Include("tUserSourceService.tSourceService")
                                            .Include("tMedicationForm")
                                            .Include("tUnitsOfMeasure")
                                            .Include("tUnitsOfMeasure1")
                                            .Include("tMedicationRoute")
                                            .Include("tPharmacy")
                                            .Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserPrescription item in allUserPrescriptions)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            ProviderName = item.tProvider != null ? item.tProvider.Name : string.Empty,
                            MedFormName = item.tMedicationForm != null ? item.tMedicationForm.ShortName : string.Empty,
                            FrequencyUOMName = item.tUnitsOfMeasure != null ? item.tUnitsOfMeasure.UnitOfMeasure : string.Empty,
                            StrengthUOMName = item.tUnitsOfMeasure1 != null ? item.tUnitsOfMeasure1.UnitOfMeasure : string.Empty,
                            RouteName = item.tMedicationRoute != null ? item.tMedicationRoute.ShortName : string.Empty,
                            PharmacyName = item.tPharmacy != null ? item.tPharmacy.Name : string.Empty,
                            Name = item.Name,
                            Instructions = item.Instructions,
                            ProductName = item.ProductName,
                            BrandName = item.BrandName,
                            DosageText = item.DosageText,
                            DosageValue = item.DosageValue,
                            FrequencyValue = item.FrequencyValue,
                            StrengthValue = item.StrengthValue,
                            RefillsRemaining = item.RefillsRemaining,
                            RefillsTotal = item.RefillsTotal,
                            ExpirationDateTime = item.ExpirationDateTime,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Problems":
                    #region Problems
                    List<tUserProblem> allUserProblems = db.tUserProblems.Include("tUserSourceService.tSourceService").Include("tSourceOrganization.tOrganization").Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserProblem item in allUserProblems)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Name = item.Name,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Procedures":
                    #region Procedures
                    List<tUserProcedure> allUserProcedures = db.tUserProcedures
                                            .Include("tUserSourceService.tSourceService")
                                            .Include("tSourceOrganization.tOrganization")
                                            .Include("tSourceOrganization1.tOrganization")
                                            .Include("tUserProcedureDevice")
                                            .Include("tUserSpecimen")
                                            .Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserProcedure item in allUserProcedures)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            StartDateTime = item.StartDateTime,
                            EndDateTime = item.EndDateTime,
                            Name = item.Name,
                            PerformingOrganizationName = item.PerformingOrganizationID != null ? item.tSourceOrganization1.tOrganization.Name : "",
                            DeviceName = item.DeviceID != null ? item.tUserProcedureDevice.Name : "",
                            SpecimenName = item.SpecimenID != null ? item.tUserSpecimen.Specimen : "",
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Test Results":
                    #region Test Results
                    List<tUserTestResult> allUserTestResult = db.tUserTestResults
                                            .Include("tUserSourceService.tSourceService")
                                            .Include("tSourceOrganization.tOrganization")
                                            .Include("tProvider")
                                            .Include("tTestResultStatus")
                                            .Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserTestResult item in allUserTestResult)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            ResultDateTime = item.ResultDateTime,
                            Name = item.Name,
                            Comments = item.Comments,
                            Narrative = item.Narrative,
                            Impression = item.Impression,
                            Transcriptions = item.Transcriptions,
                            ProviderName = item.OrderingProviderID != null ? item.tProvider.Name : "",
                            Status = item.StatusID != null ? item.tTestResultStatus.Status : "",
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Vitals":
                    #region Vitals
                    List<tUserVital> allUserVitals = db.tUserVitals
                                            .Include("tUserSourceService.tSourceService")
                                            .Include("tSourceOrganization.tOrganization")
                                            .Include("tProvider")
                                            .Include("tUnitsOfMeasure")
                                            .Where(a => a.UserID == userId && a.SystemStatusID == 1).ToList();
                    foreach (tUserVital item in allUserVitals)
                    {
                        tUserDerivedNote _userNote = db.tUserDerivedNotes.ToList().FirstOrDefault(a => a.fkObjectID == item.ObjectID);
                        string _source = string.Empty;
                        if (item.SourceOrganizationID == null)
                        {
                            _source = item.tUserSourceService != null ? item.tUserSourceService.tSourceService.ServiceName : "";
                        }
                        else
                        {
                            _source = item.tSourceOrganization.tOrganization.Name;
                        }
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            ID = _userNote != null ? _userNote.ID : -1,
                            Note = _userNote != null ? _userNote.Note : string.Empty,
                            ResultDateTime = item.ResultDateTime,
                            Name = item.Name,
                            Value = item.Value,
                            ProviderName = item.ProviderID != null ? item.tProvider.Name : "",
                            UOMName = item.tUnitsOfMeasure.UnitOfMeasure,
                            Source = _source,
                            ParentId = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;

                default:
                    break;
            }


            return retVal;
        }

        // GET: api/GetUserDataWithNotes/5/Allergies
        [Route("api/CustomUserData/GetUserDataWithNotes/{categoryId}/{category}")]
        [ResponseType(typeof(Models.UserData.UserDataWithNote))]
        public async Task<IHttpActionResult> GetUserDataWithNotes(int categoryid, string category)
        {

            Models.UserData.UserDataWithNote _retval = new Models.UserData.UserDataWithNote();
            tUserDerivedNote _tUserDerivedNote = null;
            switch (category)
            {
                case "Allergies":
                    #region Allergies
                    tUserAllergy _userAllergy = await db.tUserAllergies.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userAllergy.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userAllergy.ID,
                        EndDateTime = _userAllergy.EndDateTime,
                        StartDateTime = _userAllergy.StartDateTime,
                        AllergenID = _userAllergy.AllergenID,
                        ReactionID = _userAllergy.ReactionID,
                        SeverityID = _userAllergy.SeverityID,
                        StatusID = _userAllergy.StatusID,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Care Plans":
                    #region Care Plans
                    tUserCarePlan _userCarePlan = await db.tUserCarePlans.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userCarePlan.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userCarePlan.ID,
                        EndDateTime = _userCarePlan.EndDateTime,
                        StartDateTime = _userCarePlan.StartDateTime,
                        CarePlanTypeID = _userCarePlan.TypeID,
                        CarePlanSpecialtyID = _userCarePlan.SpecialtyID,
                        CarePlanName = _userCarePlan.Name,
                        CarePlanText = _userCarePlan.Text,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Encounters":
                    #region Encounters
                    tUserEncounter _userEncounters = await db.tUserEncounters.FindAsync(categoryid);
                    tUserInstruction _FollowUpIns = await db.tUserInstructions.FindAsync(_userEncounters.FollowUpInstructionID);
                    tUserInstruction _PatientIns = await db.tUserInstructions.FindAsync(_userEncounters.PatientInstructionID);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userEncounters.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userEncounters.ID,
                        EncounterDateTime = _userEncounters.EncounterDateTime,
                        ProviderID = _userEncounters.ProviderID,
                        VisitTypeID = _userEncounters.VisitTypeID,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty,
                        FollowUpInstruction = _FollowUpIns,
                        PatientInstruction = _PatientIns
                    };
                    #endregion
                    break;
                case "Functional Statuses":
                    #region Functional Statuses
                    tUserFunctionalStatus _userFunctionalStatus = await db.tUserFunctionalStatuses.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userFunctionalStatus.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userFunctionalStatus.ID,
                        EndDateTime = _userFunctionalStatus.EndDateTime,
                        StartDateTime = _userFunctionalStatus.StartDateTime,
                        Name = _userFunctionalStatus.Name,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Immunizations":
                    #region Immunizations
                    tUserImmunization _userImmunization = await db.tUserImmunizations.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userImmunization.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userImmunization.ID,
                        Name = _userImmunization.Name,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Instructions":
                    #region Instruction
                    tUserInstruction _userInstruction = await db.tUserInstructions.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userInstruction.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userInstruction.ID,
                        Name = _userInstruction.Name,
                        Text = _userInstruction.Text,
                        StartDateTime = _userInstruction.StartDateTime,
                        EndDateTime = _userInstruction.EndDateTime,
                        InstructionTypeID = _userInstruction.InstructionTypeID,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Narratives":
                    #region Narratives
                    tUserNarrative _userNarrative = await db.tUserNarratives.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userNarrative.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userNarrative.ID,
                        ProviderID = _userNarrative.ProviderID,
                        StartDateTime = _userNarrative.StartDateTime,
                        EndDateTime = _userNarrative.EndDateTime,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Prescriptions":
                    #region Prescriptions
                    tUserPrescription _userPrescription = await db.tUserPrescriptions.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userPrescription.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        ParentId = _userPrescription.ID,
                        ProviderID = _userPrescription.ProviderID,
                        MedFormID = _userPrescription.MedFormID,
                        FrequencyUOMID = _userPrescription.FrequencyUOMID,
                        StrengthUOMID = _userPrescription.StrengthUOMID,
                        RouteID = _userPrescription.RouteID,
                        PharmacyID = _userPrescription.PharmacyID,
                        Name = _userPrescription.Name,
                        Instructions = _userPrescription.Instructions,
                        ProductName = _userPrescription.ProductName,
                        BrandName = _userPrescription.BrandName,
                        DosageText = _userPrescription.DosageText,
                        DosageValue = _userPrescription.DosageValue,
                        FrequencyValue = _userPrescription.FrequencyValue,
                        StrengthValue = _userPrescription.StrengthValue,
                        RefillsRemaining = _userPrescription.RefillsRemaining,
                        RefillsTotal = _userPrescription.RefillsTotal,
                        ExpirationDateTime = _userPrescription.ExpirationDateTime,
                        StartDateTime = _userPrescription.StartDateTime,
                        EndDateTime = _userPrescription.EndDateTime,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Problems":
                    #region Problems
                    tUserProblem _userProblem = await db.tUserProblems.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userProblem.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        Name = _userProblem.Name,
                        ParentId = _userProblem.ID,
                        StartDateTime = _userProblem.StartDateTime,
                        EndDateTime = _userProblem.EndDateTime,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Procedures":
                    #region Procedures
                    tUserProcedure _userProcedure = await db.tUserProcedures.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userProcedure.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        Name = _userProcedure.Name,
                        ParentId = _userProcedure.ID,
                        PerformingOrganizationID = _userProcedure.PerformingOrganizationID,
                        DeviceID = _userProcedure.DeviceID,
                        SpecimenID = _userProcedure.SpecimenID,
                        StartDateTime = _userProcedure.StartDateTime,
                        EndDateTime = _userProcedure.EndDateTime,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Test Results":
                    #region Test Results
                    tUserTestResult _userTestResult = await db.tUserTestResults.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userTestResult.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        Name = _userTestResult.Name,
                        Comments = _userTestResult.Comments,
                        Narrative = _userTestResult.Narrative,
                        Impression = _userTestResult.Impression,
                        Transcriptions = _userTestResult.Transcriptions,
                        ParentId = _userTestResult.ID,
                        ProviderID = _userTestResult.OrderingProviderID,
                        StatusID = _userTestResult.StatusID,
                        ResultDateTime = _userTestResult.ResultDateTime,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                case "Vitals":
                    #region Test Results
                    tUserVital _userVital = await db.tUserVitals.FindAsync(categoryid);
                    _tUserDerivedNote = db.tUserDerivedNotes.FirstOrDefault(a => a.fkObjectID == _userVital.ObjectID);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _tUserDerivedNote != null ? _tUserDerivedNote.ID : -1,
                        Name = _userVital.Name,
                        Value = _userVital.Value,
                        ParentId = _userVital.ID,
                        ProviderID = _userVital.ProviderID,
                        UOMID = _userVital.UOMID,
                        ResultDateTime = _userVital.ResultDateTime,
                        Note = _tUserDerivedNote != null ? _tUserDerivedNote.Note : string.Empty
                    };
                    #endregion
                    break;
                default:
                    break;
            }
            return Ok(_retval);
        }

        // GET: api/GetSubDataByParentIdAndCategory/123/Allergies
        [Route("api/CustomUserData/GetSubDataByParentIdAndCategory/{parentId}/{category}")]
        public List<Models.UserData.UserDataWithNote> GetSubDataByParentIdAndCategory(int parentId, string category)
        {
            // System status Id == 1 - Valid Entry
            List<Models.UserData.UserDataWithNote> retVal = new List<Models.UserData.UserDataWithNote>();
            switch (category)
            {

                case "Immunizations":
                    #region Immunizations
                    List<tUserImmunizationsDate> allUserImmunizationDates = db.tUserImmunizationsDates.Where(a => a.UserImmunizationID == parentId && a.SystemStatusID == 1).ToList();
                    foreach (tUserImmunizationsDate item in allUserImmunizationDates)
                    {
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {
                            Date = item.DateTime,
                            ParentId = item.UserImmunizationID,
                            ID = item.ID
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Narratives":
                    #region Narratives Entry
                    List<tUserNarrativeEntry> allUserNarrativeEntry = db.tUserNarrativeEntries.Where(a => a.NarrativeID == parentId && a.SystemStatusID == 1).ToList();
                    foreach (tUserNarrativeEntry item in allUserNarrativeEntry)
                    {
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {

                            ParentId = item.NarrativeID,
                            ID = item.ID,
                            Text = item.SectionText,
                            Title = item.SectionTitle,
                            SeqNum = item.SectionSeqNum
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                case "Test Results":
                    #region Test Results Component
                    List<tUserTestResultComponent> allUserTestResultComponent = db.tUserTestResultComponents.Include("tUnitsOfMeasure")
                        .Where(a => a.TestResultID == parentId && a.SystemStatusID == 1).ToList();
                    foreach (tUserTestResultComponent item in allUserTestResultComponent)
                    {
                        Models.UserData.UserDataWithNote _item = new Models.UserData.UserDataWithNote()
                        {

                            ParentId = item.TestResultID,
                            ID = item.ID,
                            UOMName = item.UOMID != null ? item.tUnitsOfMeasure.UnitOfMeasure : "",
                            Name = item.Name,
                            ComponentsValue = item.Value,
                            LowValue = item.LowValue,
                            HighValue = item.HighValue,
                            RefRange = item.RefRange,
                            Comments = item.Comments
                        };
                        retVal.Add(_item);
                    }
                    #endregion
                    break;
                default:
                    break;
            }


            return retVal;
        }

        // GET: api/GetSubDataByCategory/5/Allergies
        [Route("api/CustomUserData/GetSubDataByCategory/{itemId}/{category}")]
        [ResponseType(typeof(Models.UserData.UserDataWithNote))]
        public async Task<IHttpActionResult> GetSubDataByCategory(int itemId, string category)
        {

            Models.UserData.UserDataWithNote _retval = new Models.UserData.UserDataWithNote();
            switch (category)
            {

                case "Immunizations":
                    #region Immunizations
                    tUserImmunizationsDate _userImmunizationDate = await db.tUserImmunizationsDates.FindAsync(itemId);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _userImmunizationDate.ID,
                        ParentId = _userImmunizationDate.UserImmunizationID,
                        Date = _userImmunizationDate.DateTime
                    };
                    #endregion
                    break;
                case "Narratives":
                    #region Narrative Entry
                    tUserNarrativeEntry _userNarrativeEntry = await db.tUserNarrativeEntries.FindAsync(itemId);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _userNarrativeEntry.ID,
                        ParentId = _userNarrativeEntry.NarrativeID,
                        SeqNum = _userNarrativeEntry.SectionSeqNum,
                        Text = _userNarrativeEntry.SectionText,
                        Title = _userNarrativeEntry.SectionTitle
                    };
                    #endregion
                    break;
                case "Test Results":
                    #region Test Results Component
                    tUserTestResultComponent _userTestResultComponent = await db.tUserTestResultComponents.FindAsync(itemId);
                    _retval = new Models.UserData.UserDataWithNote()
                    {
                        ID = _userTestResultComponent.ID,
                        ParentId = _userTestResultComponent.TestResultID,
                        UOMID = _userTestResultComponent.UOMID != null ? _userTestResultComponent.UOMID.Value : 0,
                        Name = _userTestResultComponent.Name,
                        ComponentsValue = _userTestResultComponent.Value,
                        LowValue = _userTestResultComponent.LowValue,
                        HighValue = _userTestResultComponent.HighValue,
                        RefRange = _userTestResultComponent.RefRange,
                        Comments = _userTestResultComponent.Comments
                    };
                    #endregion
                    break;
                default:
                    break;
            }
            return Ok(_retval);
        }
    }
}
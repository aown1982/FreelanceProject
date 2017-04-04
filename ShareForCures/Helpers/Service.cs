using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShareForCures.Helpers
{
    public sealed class Service
    {
        public static readonly string REST_API_SERVER = System.Configuration.ConfigurationManager.AppSettings["RESTApi"].ToString();
        public static readonly string HUMAN_API_SERVER = "https://user.humanapi.co";
        public static readonly string SOCIAL_TYPE_FACEBOOK = "FACEBOOK";
        public static readonly string SOCIAL_TYPE_TWITTER = "TWITTER";
        public static readonly string SOCIAL_TYPE_GOOGLE = "GOOGLE";
        public static readonly string USER_ENTERED = "USER ENTERED";
        public static readonly string ADD_HUMAN_API = "v1/connect/tokens/";

        #region "Users API communication"
        public static readonly string LOGIN_USER = "Users/Login";
        public static readonly string DBUSERS_GET_COUNTRIES = "users/countries";
        public static readonly string DBUSERS_REGISTER_USER = "users/RegisterUser";
        public static readonly string GET_USER_BY_EMAIL = "users/GetUserByEmail/";
        public static readonly string GET_USER_BY_ID = "users/GetUserById/";
        public static readonly string UPDATE_USER = "Users/UpdateUser/";
        public static readonly string UPDATE_USER_PASSWORD_RESET = "Users/UpdateUserPasswordReset/";
        public static readonly string UPDATE_USER_BASIC_INFO = "Users/UpdateUserBasicInfo/";
        public static readonly string VALIDATE_PASSWORD = "Users/ValidatePassword";
        public static readonly string Get_AuditLogRepot = "Users/GetAuditLogReport/";
        public static readonly string GetSurveyChartData = "WebApp/GetSurveyChartData/";
        public static readonly string GET_USER_PASSWORD_RESET_CODE = "Users/GetUserPasswordResetCode";
        public static readonly string GET_USER_PASSWORD_RESET = "Users/GetUserPasswordReset";


        #endregion

        #region "UsersAudits API Communication"
        #endregion


        #region "UserData API Communication"
        public static readonly string GET_ALL_SOURCE = "UserData/GetSources/";
        public static readonly string GET_SOCIAL_SOURCE_BY_NAME = "UserData/GetSocialSourceByName/";
        public static readonly string GET_SOURCE_SERVICE_TYPE_SOCIAL = "UserData/GetSourcesServiceTypeSocial";
        public static readonly string GET_SOURCE_SERVICE_BY_TYPE_AND_SOURCE_ID = "UserData/GetSourceServicesBySourceIDAndType/";
        public static readonly string GET_CREDENTIALS_BY_USER_ID = "UserData/GetCredentialByUserId/";
        public static readonly string GET_PUBLICTOKEN_BY_USER_ID = "UserData/GetPublicTokenByUserId/";
        public static readonly string GET_CREDENTIALS_BY_SOURCE_USER_ID = "UserData/GetCredentialBySourceUserId/";
        public static readonly string ADD_NEW_CREDENTIALS = "UserData/PostCredentials";
        public static readonly string UPDATE_CREDENTIALS = "UserData/UpdateCredentials/";
        public static readonly string GetSourceChartData = "UserData/GetSourceConnectedChartData/";



        public static readonly string SET_USER_WEIGHT_GOAL = "UserData/SetUserWeightGoal/";
        public static readonly string SET_USER_DIET_GOAL = "UserData/SetUserDietGoal/";
        public static readonly string SET_USER_EXERCISE_GOAL = "UserData/SetUserExerciseGoal/";
        public static readonly string SET_USER_SLEEP_GOAL = "UserData/SetUserSleepGoal/";

        public static readonly string GET_USER_LAST5WEIGHT_VITALS = "UserData/GetUserLast5WeightVitals/";
        public static readonly string GET_USER_WEIGHT_VITALS = "UserData/GetUserWeightVitals/";
        public static readonly string GET_USER_WEIGHT_GOAL_VITAL = "UserData/GetUserWeightGoalVital/";
        public static readonly string GET_USER_LAST5EXERCISE_VITALS = "UserData/GetUserLast5ExerciseVitals/";
        public static readonly string GET_USER_EXERCISE_VITALS = "UserData/GetUserExerciseVitals/";
        public static readonly string GET_USER_EXERCISE_GOAL_VITAL = "UserData/GetUserExerciseGoalVital/";
        public static readonly string GET_USER_LAST5DIET_VITALS = "UserData/GetUserLast5DietVitals/";
        public static readonly string GET_USER_DIET_VITALS = "UserData/GetUserDietVitals/";
        public static readonly string GET_USER_DIET_GOAL_VITAL = "UserData/GetUserDietGoalVital/";
        public static readonly string GET_USER_LAST5SLEEP_VITALS = "UserData/GetUserLast5SleepVitals/";
        public static readonly string GET_USER_SLEEP_VITALS = "UserData/GetUserSleepVitals/";
        public static readonly string GET_USER_SLEEP_GOAL_VITAL = "UserData/GetUserSleepGoalVital/";

        public static readonly string GET_USER_BLOOD_GLUCOSE = "UserData/GetUserBloodGlucose/";
        public static readonly string GET_USER_BLOOD_PRESSURE = "UserData/GetUserBloodPressure/";
        public static readonly string GET_USER_CURRENT_BLOOD_PRESSURE = "UserData/GetUserCurrentBloodPressure/";
        public static readonly string GET_USER_BODY_COMPOSITION = "UserData/GetUserBodyComposition/";
        public static readonly string GET_USER_CHOLESTEROL = "UserData/GetUserCholesterol/";
        public static readonly string GET_USER_TREND_BLOOD_GLUCOSE = "UserData/GetUserTrendBloodGlucose/";
        public static readonly string GET_USER_TREND_BLOOD_PRESSURE = "UserData/GetUserTrendBloodPressure/";
        public static readonly string GET_USER_TREND_BODY_COMPOSITION = "UserData/GetUserTrendBodyComposition/";
        public static readonly string GET_USER_TREND_WEIGHT = "UserData/GetUserTrendWeight/";
        public static readonly string GET_USER_TREND_CHOLESTEROL = "UserData/GetUserTrendCholesterol/";
        public static readonly string GET_USER_TREND_BLOOD_GLUCOSE_ACCOUNTS = "UserData/GetUserTrendBloodGlucoseAccounts/";
        public static readonly string GET_USER_TREND_BLOOD_PRESSURE_ACCOUNTS = "UserData/GetUserTrendBloodPressureAccounts/";
        public static readonly string GET_USER_TREND_BODY_COMPOSITION_ACCOUNTS = "UserData/GetUserTrendBodyCompositionAccounts/";
        public static readonly string GET_USER_TREND_CHOLESTEROL_ACCOUNTS = "UserData/GetUserTrendCholesterolAccounts/";
        public static readonly string GET_USER_TREND_DIET_ACCOUNTS = "UserData/GetUserTrendDietAccounts/";
        public static readonly string GET_USER_TREND_WEIGHT_ACCOUNTS = "UserData/GetUserTrendWeightAccounts/";
        public static readonly string GET_USER_TREND_ACTIVITY_ACCOUNTS = "UserData/GetUserTrendActivityAccounts/";
        public static readonly string GET_USER_TREND_DIET = "UserData/GetUserTrendDiet/";



        public static readonly string GET_ALL_TEST_RESULT_STATUS = "UserData/GetTestResultStatus/";
        public static readonly string GET_ALL_UOM = "UserData/GetUnitsOfMeasures";
        public static readonly string GET_ALL_DIET_CATEGORY = "UserData/GetDietCategories";
        public static readonly string GET_ALL_ACTIVITIES = "UserData/GetActivities";
        public static readonly string GET_ALL_LANGUAGES = "Users/Languages";
        public static readonly string GET_ALL_STATES = "Users/States";
        public static readonly string GET_ALL_NUTRIENT = "UserData/GetNutrients";

        public static readonly string SOFTDELETE_USERACTIVITY = "UserData/SoftDeleteUserActivity/";
        public static readonly string ADD_NEW_USER_ACTIVITY = "UserData/PostUserActivities/";

        public static readonly string SOFTDELETE_USERDIET = "UserData/SoftDeleteUserDiet/";
        public static readonly string ADD_NEW_USER_DIET = "UserData/AddUserDiets/";
        public static readonly string GET_USER_DIET_NUTRIENT = "UserData/GetUserDietNutrient/";
        public static readonly string SOFTDELETE_USERDIET_NUTRIENT = "UserData/SoftDeleteUserDietNutrient/";
        public static readonly string ADD_NEW_USER_DIET_NUTRIENT = "UserData/PostXrefUserDietNutrients/";
        

        public static readonly string SOFTDELETE_TUSERTESTRESULT = "UserData/SoftDeleteUserTestResult/";
        public static readonly string SOFTDELETE_TUSERTESTRESULTCOMPONENT = "UserData/SoftDeleteUserTestResultComponent/";
        public static readonly string ADD_NEW_tUSERTESTRESULT = "UserData/PostUserTestResults";
        public static readonly string ADD_NEW_tUSERTESTRESULTCOMPONENT = "UserData/PostUserTestResultComponents";
        public static readonly string ADD_NEW_tUSERTESTRESULTCOMPONENTCODE = "UserData/PostXrefUserTestResultComponentsCodes";
        public static readonly string GET_USER_TREND_ACTIVITY = "UserData/GetUserTrendActivity/";

        public static readonly string ADD_NEW_BLOOD_PRESSURE_USERVITAL = "UserData/PostUserVitals";
        public static readonly string ADD_NEW_WEIGHT_USERVITAL = "UserData/PostUserVitals";


        public static readonly string SOFTDELETE_TUSERVITALS = "UserData/SoftDeleteUserVitals/";
        public static readonly string ADD_NEW_BODY_COMPOSITION_USERVITAL = "UserData/PostUserVitals";
        public static readonly string UPDATE_USER_LANGUAGAE_LOCATION = "Users/UpdateUserLanguageLocation";
        public static readonly string GET_USER_SETTING_DETAILS = "Users/GetUserSettingDetails/";
        public static readonly string UPDATE_USER_DOB_GENDER = "Users/UpdateUserDobGender";
        public static readonly string GET_ALL_GENDERS = "Users/Genders";


        public static readonly string GET_ALL_Allergens = "UserData/GetAllergen/";
        public static readonly string GET_ALL_Reactions = "UserData/GetReactions/";
        public static readonly string GET_ALL_AllergyStatus = "UserData/GetAllergyStatus/";
        public static readonly string GET_ALL_AllergySeverity = "UserData/GetAllergySeverity/";
        public static readonly string ADD_NEW_tUserAllergies = "UserData/PostUserAllergies";
        public static readonly string ADD_NEW_tUserDerivedNote = "UserData/PostUserDerivedNote";
        public static readonly string GET_ALL_UserDataByCategoryWithNotes = "CustomUserData/GetUserDataByCategoryWithNotes/";
        public static readonly string GET_UserDataWithNotes = "CustomUserData/GetUserDataWithNotes/";
        public static readonly string SoftDelete_tUserAllergies = "UserData/SoftDeleteUserAllergies/";
        public static readonly string SoftDelete_tUserDerivedNote = "UserData/SoftDeletetDerivedNote/";
        public static readonly string DELETE_tUserDerivedNote = "UserData/DeleteUserDerivedNote/";
        public static readonly string Get_UserSourceServiceByUserIdAndCategory = "UserData/GetUserSourceServiceByUserIdAndCategory/";
        public static readonly string GET_ALL_CarePlanTypes = "UserData/GetUserCarePlanTypes/";
        public static readonly string GET_ALL_CarePlanSpecialties = "UserData/GetUserCarePlanSpecialties/";
        public static readonly string SoftDelete_tUserCarePlan = "UserData/SoftDeleteUserCarePlan/";
        public static readonly string ADD_NEW_tUserCarePlans = "UserData/PostUserCarePlans";
        public static readonly string GET_ALL_Providers = "UserData/GetProviders/";
        public static readonly string GET_ALL_VisitTypes = "UserData/GetVisitTypes/";
        public static readonly string GET_ALL_InstructionTypes = "UserData/GetInstructionTypes/";
        public static readonly string ADD_NEW_tUserEncounters = "UserData/PostUserEncounters";
        public static readonly string SoftDelete_tUserEncounter = "UserData/SoftDeleteUserEncounter/";
        public static readonly string ADD_NEW_tUserInstruction = "UserData/PostUserInstructions";
        public static readonly string SoftDelete_tUserFunctionalStatuses = "UserData/SoftDeleteUserFunctionalStatus/";
        public static readonly string ADD_NEW_tUserFunctionalStatuses = "UserData/PostUserFunctionalStatuses";
        public static readonly string SoftDelete_tUserImmunization = "UserData/SoftDeleteUserImmunization/";

        public static readonly string ADD_NEW_tUserImmunization = "UserData/PostUserImmunizations";
        public static readonly string SoftDelete_tUserInstruction = "UserData/SoftDeleteUserInstruction/";
        public static readonly string SoftDelete_tUserNarrative = "UserData/SoftDeleteUserNarrative/";
        public static readonly string ADD_NEW_tUserNarrative = "UserData/PostUserNarrative";
        public static readonly string GET_ALL_MedicationForms = "UserData/GetMedicationForms/";
        public static readonly string GET_ALL_UnitsOfMeasures = "UserData/GetUnitsOfMeasures/";
        public static readonly string GET_ALL_Pharmacies = "UserData/GetPharmacies/";
        public static readonly string GET_ALL_MedicationRoutes = "UserData/GetMedicationRoutes/";
        public static readonly string SoftDelete_tUserPrescription = "UserData/SoftDeleteUserPrescription/";
        public static readonly string ADD_NEW_tUserPrescriptions = "UserData/PostUserPrescriptions";
        public static readonly string SoftDelete_tUserProblem = "UserData/SoftDeleteUserProblem/";
        public static readonly string ADD_NEW_tUserProblems = "UserData/PostUserProblems";
        public static readonly string GET_ALL_SourceOrganizations = "UserData/GetSourceOrganizations/";
        public static readonly string GET_ALL_ProcedureDevices = "UserData/GetUserProcedureDevices/";
        public static readonly string GET_ALL_Specimens = "UserData/GetUserSpecimens/";
        public static readonly string SoftDelete_UserProcedure = "UserData/SoftDeleteUserProcedure/";
        public static readonly string ADD_NEW_UserProcedures = "UserData/PostUserProcedures";
        public static readonly string ADD_NEW_UserImmunizationsDates = "UserData/PostUserImmunizationsDates";
        public static readonly string GET_ALL_SubDataByParentIdAndCategory = "CustomUserData/GetSubDataByParentIdAndCategory/";
        public static readonly string GET_SubDataByCategory = "CustomUserData/GetSubDataByCategory/";
        public static readonly string SoftDelete_UserImmunizationsDate = "UserData/SoftDeleteUserImmunizationsDate/";
        public static readonly string ADD_NEW_UserNarrativeEntry = "UserData/PostUserNarrativeEntries";
        public static readonly string SoftDelete_UserNarrativeEntry = "UserData/SoftDeleteUserNarrativeEntry/";

        #endregion

        #region Surveys API Communication
        public static readonly string Get_Surveys = "WebApp/GetSurveysByUser/";
        public static readonly string Get_SurveyQuestion = "WebApp/GetSurveyQuestions/";
        public static readonly string Save_UserSurveyResults = "UserData/PostUserSurveyResults/";
        public static readonly string Get_UserSurveyResults = "UserData/GetUserSurveyResultsData/";
        public static readonly string Get_AdsByUserId = "WebApp/GetAdsByUserId/";
        public static readonly string Save_AdsLogData = "WebApp/PostAdsLog";
        public static readonly string Get_SourceSeviceType = "UserData/GetSourcesServiceTypeSocial/";
        public static readonly string Get_SharePurposeData = "UserData/GetSharePurposeData/";
        public static readonly string Save_UserShareData = "UserData/PostUserShareData";



        #endregion

        #region "WebApp API Communication"
        public static readonly string GET_INVITATION_BY_CODE = "WebApp/GetInvitationByCode/";
        public static readonly string UPDATE_INVITATION_CODE = "WebApp/EditInvitationCode/";
        public static readonly string ADD_WEBAPP_ERROR = "WebApp/WAErrLogs/Add";


        #endregion


        #region SHARE Settings

        public static readonly string GET_USER_SHAREALLOWS = "UserData/GetUserSHAREAllowsByUserID";
        public static readonly string GET_USER_SHAREDENIES = "UserData/GetUserSHAREDeniesByUserID";

        #endregion



        #region Overview - Recent Activity & Relevant Information
        public static readonly string Get_ListOfAuditTypesByUser = "UserData/GetListOfAuditTypesByUser/";

        #endregion
    }
}
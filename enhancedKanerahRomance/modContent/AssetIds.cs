using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace enhancedKanerahRomance.modContent
{
    public static class AssetIds
    {
        // assetIds must begin with new for duplicate checking to work

        // existing assetIds
        // START the assetId with the type of thing it is e.g., cue, cutscene, etc

        // companions and related bits
        public const string unitKanerah = "f1c0b181a534f4940ae17f243a5968ec";
        public const string unitKalikke = "c807d18a89f96c74f8bb48b31b616323";

        // dialog, cues, answers, answersLists etc
        public const string cueWantYouToLeaveMyLands = "5affbca409d60e34a8af9ad9ef4b0e11";
        public const string answerNoMoreIntimate = "695d2f0b36c8a2446aba22c09f1e5dbb";
        public const string answersListWhatItMeansToBeATiefling = "32e8b78bc87dc944bb43612e97419b3f";
        public const string dialogTwinsCompanion = "cc84ee93d2f328c48a7747e7e8e8a234";
        
        // areas and related bits
        public const string areaCapitalTavern_Indoor = "5c3935c8ab777f04f83f272425b750f9";
        public const string componentListCapitalSquareAreaMechanics = "90b7e95ae946cc542bc94c75d931ee22";
        public const string areaEnterPointKanerahRoom = "e22132a384c243e4eb1223f8623a777f";
        public const string locatorKanerahRoom = "f31018ae-91fd-4088-85b8-c003da63e4f1";

        // flags
        public const string flagStoneCapital = "1795eee9e31c4414dbedb67bd700ee97";

        // cutscenes
        public const string cutsceneGenericRomanceEvent = "a2c8aacd03bba4e41b4c6ccac15bee8b";
        public const string cutsceneKanerahSex = "5004819ce74793e41a92a7ab700021f4";

        // testcase assetIds
        public const string newTestCounterFlag = "11cdd5ef5beb4891b51ba9fd97f338a4";
        public const string newTestUnlockedFlag = "7447b84aaa444485859eb17f6b41c560";
        public const string newTestUnlockedFlag2 = "294cb068d11e4080a6939530077f28ff";

        public const string newCueTestCase1 = "e5b3de8f8e0640a4a632c219c44b20d2";
        public const string newCueTestCase2 = "ff42c96b0e07428fb87ce9f2e4f4942a";
        public const string newCueTestCase3 = "ba8ad39943774f17a13c1ca9a8f2d45c";
        public const string newCueTestCase4 = "36d18060671842d289b0aa7005a8c201";
        public const string newCueTestCase5 = "df0bf3446c9c4373941141424ec4db4f";
        public const string newCueTestCase6Success = "605695a03ccf48c783b54e7f9ad987a4";
        public const string newCueTestCase7Fail = "242b48fc5fd54459a5872860b13fc2b8";
        public const string newCueTestCase8 = "8e1a29658b3049ffb4e132585d9793b3";
        public const string newCueTestCase9 = "27314db8320147a5ad7e5291a22cd30e";
        public const string newCueTestCase10 = "69f122c03e864d6489245c6cb2955145";
        public const string newCueTestCase11 = "32d9e28b7f834928bfdd8da20214f3b4";
        public const string newCueTestCase12 = "bef7c1e37aeb4648aa3998f3370dc4d1";
        public const string newCueTestCase13 = "2febfc371ea344d19408908825946329";

        public const string newCheckTestCase1 = "268ce569ebca4f9cae34af309475c73c";

        public const string newAnswerTestCase1 = "0c9d8cd3443b40499e954e3f7341828a";
        public const string newAnswerTestCase2 = "3f70bd99e1dc4c09accb08d6f2c9166f";
        public const string newAnswerTestCase3 = "eeb43abe2aa44f2e90f84820b58d5c11";
        public const string newAnswerTestCase4 = "620a2be6c9b4400ab9a29fa83c9ae2f6";
        public const string newAnswerTestCase5 = "82bf49febee44ac0ae1115f5ba77d80b";
        public const string newAnswerTestCase6 = "5ced3a1ba1dc470a9043e3dbde6146dd";
        public const string newAnswerTestCase7 = "bade53cff7b0441bad466cc8acacf575";
        public const string newAnswerTestCase8 = "f4f5786a3ec744c889e246f5325db3cd";
        public const string newAnswerTestCase9 = "7318c05d131f46369c21fe10266fd307";
        public const string newAnswerTestCase10 = "16b6a64091854f8098695c8e79e07fc7";
        public const string newAnswerTestCase11 = "830a29c9efb847bf88fc65161c1e1bfb";
        public const string newAnswerTestCase12 = "c453bd8a97f246e89214041f7a59679c";
        public const string newAnswerTestCase13 = "de3430361dca41bba4541704b4cab734";
        public const string newAnswerTestCase14 = "4fd98914d98e4da3bdb9b385b0b4109e";
        public const string newAnswerTestCase15 = "8b372093b33e4063bc3fc4b63ed0a20f";

        public const string newAnswersListTestCase1 = "d10dbd690b3f45b696f35731bf58554b";
        public const string newAnswersListTestCase2 = "19d967a222754ecc8877b7fca0fafe56";
        public const string newAnswersListTestCase3 = "2ba9a1a86a244cf5bd422b575e311aeb";

        public const string newLawfulShiftTestCase = "061cb70dcfac4c97876ae894c2f6d013";

        public const string newCampingEncounterTestCase1 = "5b9c2cfce9e14aeba0a278675a606985";

        public const string newDialogForCampingEncounterTestCase1 = "973bcaa1246f4ef28f42b1433eb771a7";
        public const string newDialogForActivateTriggerTestCase1 = "9dbd6c86a8f64310a2a41c91faf7f57b";
        public const string newDialogForActivateTriggerTestCase2 = "8de847d3e37f4d43a90a89214ad686af";

        public const string newBarkString1 = "467cd1afe7424922b0b6376117cee743";
        public const string newBarkString2 = "79ce97a33967407095fbc854cd2f83a4";
    }

}


using Microsoft.Speech.Recognition;
using Ticketer.TTS;

namespace Ticketer.ASR.Validators
{
    class RecognitionValidator
    {
        private static readonly Speaker speaker = new Speaker();
        public static bool CorrectlyRecognized(RecognitionResult recognition)
        {
            if(recognition == null)
            {
                return false;
            } else
            {
                if(recognition.Confidence < 0.5)
                {
                    speaker.AskForRepeat();
                    return false;
                } else
                {
                    return true;
                }
            }
        }
    }
}

using Microsoft.Speech.Recognition;
using Ticketer.ASR.Validators;

namespace Ticketer.ASR
{
    class AnswerReceiver
    {
        public static string GetAnswer(SpeechRecognitionEngine pSRE)
        {
            RecognitionResult recognition;
            do
            {
                recognition = pSRE.Recognize();
            } while (!RecognitionValidator.CorrectlyRecognized(recognition));
            return recognition.Text;
        }
    }
}

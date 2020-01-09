using Microsoft.Speech.Recognition;

namespace Ticketer.ASR
{
    abstract class ASRAbstract
    {
        public abstract string GetAnswer();

        protected abstract void PrepareGrammar(SpeechRecognitionEngine engine);
    }
}

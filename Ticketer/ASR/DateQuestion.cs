using Microsoft.Speech.Recognition;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class DateQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public DateQuestion()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
        }

        public override string GetAnswer()
        {
            return AnswerReceiver.GetAnswer(pSRE);
        }

        protected override void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices choices = new Choices();
            choices.Add("Poniedziałek");
            choices.Add("Wtorek");
            choices.Add("Środa");
            choices.Add("Czwartek");
            choices.Add("Piątek");
            choices.Add("Sobota");
            choices.Add("Niedziela");
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }
    }
}

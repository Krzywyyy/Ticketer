using Microsoft.Speech.Recognition;
using Ticketer.MyProperties;

namespace Ticketer.ASR
{
    class TimeQuestion : ASRAbstract
    {
        private SpeechRecognitionEngine pSRE;

        public TimeQuestion()
        {
            pSRE = new SpeechRecognitionEngine(MyCultureInfo.PolishCulture);
            pSRE.SetInputToDefaultAudioDevice();
            PrepareGrammar(pSRE);
        }

        public override string GetAnswer()
        {
            string hour = AnswerReceiver.GetAnswer(pSRE);
            return ConvertHourToValid(hour);
        }

        protected override void PrepareGrammar(SpeechRecognitionEngine engine)
        {
            GrammarBuilder grammarBuilder = new GrammarBuilder();
            Choices choices = new Choices();
            choices.Add("Trzynasta");
            choices.Add("Szesnasta");
            choices.Add("Dziewietnasta");
            grammarBuilder.Append(choices);
            Grammar grammar = new Grammar(grammarBuilder);

            engine.LoadGrammar(grammar);
        }

        private string ConvertHourToValid(string hour)
        {
            if (hour.Equals("Trzynasta"))
            {
                hour = "13:00";
            } else if (hour.Equals("Szesnasta"))
            {
                hour = "16:00";
            } else if (hour.Equals("Dziewietnasta"))
            {
                hour = "19:00";
            }
            return hour;
        }
    }
}

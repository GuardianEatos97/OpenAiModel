using OpenAiModel.Views;
namespace OpenAiModel
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute("question", typeof(YodaQuestionPage));
            Routing.RegisterRoute("answer", typeof(YodaAnswerPage));
        }
    }
}

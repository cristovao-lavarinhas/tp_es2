namespace esii.Commands
{
    public class CommandExecutor
    {
        public void Execute(ICommand command)
        {
            command.Execute();
        }
    }
}
using System;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace Tolarian.Copyshop.Fontend.WPF.Helper
{
    public static class StoryboardExtension
    {
        public static Task BeginAsync(this Storyboard storyboard)
        {
            TaskCompletionSource<bool> completionSource = new();
            if (storyboard == null)
            {
                completionSource.SetException(new ArgumentNullException());
            }
            else
            {
                void onComplete(object s, object e)
                {
                    storyboard.Completed -= onComplete;
                    completionSource.SetResult(true);
                }

                storyboard.Completed += onComplete;
                storyboard.Begin();
            }
            return completionSource.Task;
        }
    }
}
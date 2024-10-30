using System;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    public static async void DelayCall(float delay, Action action)
    {
        if (action == null)
        {
            Debug.LogWarning("Action이 null입니다.");
            return;
        }

        // 딜레이를 밀리초로 변환하여 Task.Delay 호출
        await Task.Delay(TimeSpan.FromSeconds(delay));

        // 딜레이가 끝나면 액션 실행
        action.Invoke();
    }
}
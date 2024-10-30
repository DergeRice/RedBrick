using System;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    public static async void DelayCall(float delay, Action action)
    {
        if (action == null)
        {
            Debug.LogWarning("Action�� null�Դϴ�.");
            return;
        }

        // �����̸� �и��ʷ� ��ȯ�Ͽ� Task.Delay ȣ��
        await Task.Delay(TimeSpan.FromSeconds(delay));

        // �����̰� ������ �׼� ����
        action.Invoke();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundNotification : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void RegisterForNotif()
    {
        UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert | UnityEngine.iOS.NotificationType.Badge | UnityEngine.iOS.NotificationType.Sound);
    }
    void ScheduleNotification()
    {

        // schedule notification to be delivered in 24 hours

        UnityEngine.iOS.LocalNotification notif = new UnityEngine.iOS.LocalNotification();

        notif.fireDate = System.DateTime.Now.AddSeconds(1);

        notif.alertBody = "You’ve generated more coins!Come back and play!";

        UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(notif);

    }
    void OnApplicationPause(bool isPause)

    {

        if (isPause) // App going to background

        {

            // cancel all notifications first.

#if UNITY_IOS

            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();

            UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();

            ScheduleNotification ();

#endif
        }

        else
        {

#if UNITY_IOS

            Debug.Log(“Local notification count = ” + UnityEngine.iOS.NotificationServices.localNotificationCount);

            if (UnityEngine.iOS.NotificationServices.localNotificationCount > 0) {

 

            Debug.Log(UnityEngine.iOS.NotificationServices.localNotifications[0].alertBody);

            }

            // cancel all notifications first.

            UnityEngine.iOS.NotificationServices.ClearLocalNotifications();

            UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications();

#endif

        }

    }
}

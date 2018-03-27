package com.fujiwaranosato.YokaiAR.ibeacon;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.graphics.BitmapFactory;
import android.icu.util.DateInterval;
import android.os.IBinder;
import android.os.RemoteException;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;


import org.altbeacon.beacon.Beacon;
import org.altbeacon.beacon.BeaconConsumer;
import org.altbeacon.beacon.BeaconManager;
import org.altbeacon.beacon.BeaconParser;
import org.altbeacon.beacon.Identifier;
import org.altbeacon.beacon.RangeNotifier;
import org.altbeacon.beacon.Region;
import org.altbeacon.beacon.startup.BootstrapNotifier;
import org.altbeacon.beacon.startup.RegionBootstrap;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Collection;
import java.util.Date;
import java.util.List;


public class IBeaconService extends Service implements BootstrapNotifier, BeaconConsumer {

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }


    public BeaconManager beaconManager;
    private Region region;
    private String beaconName;
    private RegionBootstrap regionBootstrap;

    private NotificationCompat.Builder notificationBuilder;
    private int notificationId = 001;
    private Notification notification;
    private NotificationManager notificationManager;

    private Context context;

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        try {
            super.onStartCommand(intent, flags, startId);

            context = IBeaconPlugin.instance().getContext();

            notificationManager = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
            PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, new Intent(context, UnityPlayerActivity.class), PendingIntent.FLAG_UPDATE_CURRENT);

            notificationBuilder = new NotificationCompat.Builder(context)
                    .setLargeIcon(BitmapFactory.decodeResource(context.getResources(), R.drawable.notification_large))
                    .setSmallIcon(R.drawable.notification_small)
                    .setContentTitle(IBeaconPlugin.title)
                    .setContentText(IBeaconPlugin.instance().getDetail())
                    .setContentIntent(pendingIntent)
                    .setAutoCancel(true);
            notification = notificationBuilder.build();
            Bind();
        } catch (Exception e) {
            e.printStackTrace();
        }
        return START_NOT_STICKY;
    }

    @Override
    public void onCreate() {
        super.onCreate();
        beaconManager = BeaconManager.getInstanceForApplication(getApplicationContext());
        beaconManager.getBeaconParsers().add(new BeaconParser().setBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24"));
        beaconManager.setBackgroundBetweenScanPeriod(2000);
        beaconManager.setBackgroundScanPeriod(2000);
        beaconManager.setForegroundBetweenScanPeriod(2000);
        beaconManager.setForegroundScanPeriod(2000);
    }

    public void Bind() {
        if (beaconManager != null)
            beaconManager.bind(this);

    }

    public void UnBind() {

        if (beaconManager != null) {
            beaconManager.unbind(this);
            beaconManager.removeAllRangeNotifiers();
            beaconManager.removeAllMonitorNotifiers();
        }
    }


    @Override
    public void onDestroy() {
        UnBind();

        super.onDestroy();

    }


    @Override
    public void didEnterRegion(Region region) {
        try {
            beaconManager.startRangingBeaconsInRegion(region);
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void didExitRegion(Region region) {
        try {
            beaconManager.stopRangingBeaconsInRegion(region);
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void didDetermineStateForRegion(int i, Region region) {
    }

    @Override
    public void onBeaconServiceConnect() {
        try {

            beaconManager.addRangeNotifier(new RangeNotifier() {
                @Override
                public void didRangeBeaconsInRegion(Collection<Beacon> beacons, Region region) {
                    if (beacons.size() > 0) {

                        for (Beacon b : beacons) {
                            for (IbeaconStruct myStruct : IBeaconPlugin.listIbeacon) {
                                if (myStruct.minor_id.equals(b.getId3().toString())
                                        && myStruct.uuid.equals(b.getId1().toString().toUpperCase())
                                        && myStruct.major_id.equals(b.getId2().toString())) {
                                    try {
                                        DateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");
                                        Date timeNow = new Date();
                                        Date timeLast = dateFormat.parse(myStruct.timeLastShown);
                                        long diff = (timeNow.getTime() - timeLast.getTime()) / 1000;
                                        if (diff >= 300) {
                                            if (IBeaconPlugin.notBackground) {
                                                String s = b.getId1() + "_" + b.getId2() + "_" + b.getId3() + "_" + diff;
                                                IBeaconPlugin.sendMessageToUnity(s);
                                            } else {
                                                notificationBuilder.setContentTitle(IBeaconPlugin.title);
                                                notification = notificationBuilder.build();
                                                notificationManager.notify(notificationId, notification);
                                            }
                                            break;
                                        }
                                    } catch (Exception e) {
                                        e.printStackTrace();
                                    }
                                }
                            }
                        }
                    }
                }
            });
        } catch (Exception e) {
            e.printStackTrace();
        }
        try {
            beaconName = "Yokai_get_ibeacon" + new Date().getTime();
            region = new Region(beaconName, null, null, null);
            beaconManager.startRangingBeaconsInRegion(region);
        } catch (RemoteException e) {
            e.printStackTrace();
        }
    }
}


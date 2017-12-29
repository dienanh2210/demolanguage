package com.yume.hhm.mylibrary;

import android.app.NotificationManager;
import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.support.annotation.Nullable;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import org.altbeacon.beacon.BeaconManager;
import org.altbeacon.beacon.BeaconParser;
import org.altbeacon.beacon.Identifier;
import org.altbeacon.beacon.Region;
import org.altbeacon.beacon.startup.BootstrapNotifier;
import org.altbeacon.beacon.startup.RegionBootstrap;

public class iBeaconService extends Service implements BootstrapNotifier{

    protected static final String TAG = "MonitoringActivity";
    private BeaconManager beaconManager;
    private Identifier identifier;
    private Region region;
    private String beaconName;
    private RegionBootstrap regionBootstrap;

    NotificationCompat.Builder mBuilder;
    int mNotificationId = 001;
    NotificationManager mNotifyMgr;

    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    public void onCreate() {
        super.onCreate();

        String CHANNEL_ID = "my_channel_01";
        mBuilder = new NotificationCompat.Builder(getApplicationContext(),CHANNEL_ID)
                .setSmallIcon(R.drawable.notification_icon)
                .setContentTitle("iBeacon detected!!!")
                .setContentText("Tap to show!!!");
        mNotifyMgr = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
        mNotifyMgr.notify(mNotificationId, mBuilder.build());

        beaconManager = BeaconManager.getInstanceForApplication(getApplicationContext());
        beaconManager.getBeaconParsers().add(new BeaconParser().setBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24"));
        beaconManager.setBackgroundBetweenScanPeriod(1000);
        identifier = Identifier.parse("B0FC4601-14A6-43A1-ABCD-CB9CFDDB4013");
        beaconName = "Yokai_get_ibeacon";
        region = new Region(beaconName, identifier, null, null);
        regionBootstrap = new RegionBootstrap(this, region);


    }

    @Override
    public void didEnterRegion(Region region) {
        Log.i(TAG, "SEE!");
        mNotifyMgr.notify(mNotificationId, mBuilder.build());
    }

    @Override
    public void didExitRegion(Region region) {
        Log.i(TAG, "DON'T SEE.");
    }

    @Override
    public void didDetermineStateForRegion(int i, Region region) {
        Log.i(TAG, "Determine State: " + i);
    }
}
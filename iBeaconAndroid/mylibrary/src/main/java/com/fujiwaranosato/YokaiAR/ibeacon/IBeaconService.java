package com.fujiwaranosato.YokaiAR.ibeacon;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.graphics.BitmapFactory;
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
import org.altbeacon.beacon.MonitorNotifier;
import org.altbeacon.beacon.Region;
import org.altbeacon.beacon.startup.BootstrapNotifier;
import org.altbeacon.beacon.startup.RegionBootstrap;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Collection;
import java.util.List;


public class IBeaconService extends Service implements BootstrapNotifier, BeaconConsumer, RangeNotifier {
    @Nullable
    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    private BeaconManager beaconManager;
    private Identifier identifier;
    private Region region;
    private String beaconName;
    private RegionBootstrap regionBootstrap;

    private NotificationCompat.Builder notificationBuilder;
    private int notificationId = 001;
    private Notification notification;
    private NotificationManager notificationManager;

    private Context context;

    private List<String> listMinor;

    public void onCreate() {
        super.onCreate();
        Log.d("ibeacon", "on create");
        try {
            context = IBeaconPlugin.instance().getContext();

            notificationManager = (NotificationManager) getSystemService(NOTIFICATION_SERVICE);
            PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, new Intent(context, UnityPlayerActivity.class), PendingIntent.FLAG_UPDATE_CURRENT);

            notificationBuilder = new NotificationCompat.Builder(context)
                    .setLargeIcon(BitmapFactory.decodeResource(context.getResources(), R.drawable.notification_large))
                    .setSmallIcon(R.drawable.notification_small)
                    .setContentTitle(IBeaconPlugin.instance().getTitle())
                    .setContentText(IBeaconPlugin.instance().getDetail())
                    .setContentIntent(pendingIntent)
                    .setAutoCancel(true);
            notification = notificationBuilder.build();

            beaconManager = BeaconManager.getInstanceForApplication(getApplicationContext());
            beaconManager.getBeaconParsers().add(new BeaconParser().setBeaconLayout("m:2-3=0215,i:4-19,i:20-21,i:22-23,p:24-24"));
            // beaconManager.setBackgroundScanPeriod(2000l);
            beaconManager.setBackgroundBetweenScanPeriod(1000l);
            beaconName = "Yokai_get_ibeacon";
            region = new Region(beaconName, null, null, null);
            regionBootstrap = new RegionBootstrap(this, region);

            beaconManager.addRangeNotifier(new RangeNotifier() {
                @Override
                public void didRangeBeaconsInRegion(Collection<Beacon> beacons, Region region) {
                    for(Beacon beacon : beacons) {
                        Log.d("ibeacon", "UUID:" + beacon.getId1() + ", major:" + beacon.getId2() + ", minor:" + beacon.getId3() + ", Distance:" + beacon.getDistance() + ",RSSI" + beacon.getRssi() + ", TxPower" + beacon.getTxPower());
                    }
                }
            });
            beaconManager.bind(this);
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void onDestroy(){
        super.onDestroy();
        // if(beaconManager != null)
            // beaconManager.unbind(this);
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d("ibeacon", "on start command");
        // regionBootstrap = new RegionBootstrap(this, region);
        if(beaconManager != null)
            // beaconManager.bind(this);
        if (intent != null && intent.getExtras() != null) {
            String json = intent.getStringExtra("json");
            parseJSON(json);
        }
        return START_NOT_STICKY;
    }

    @Override
    public void didRangeBeaconsInRegion(Collection<Beacon> beacons, Region region) {
        for (Beacon beacon : beacons) {
            Log.d("ibeacon", "UUID:" + beacon.getId1() + ", major:" + beacon.getId2() + ", minor:" + beacon.getId3() + ", Distance:" + beacon.getDistance() + ",RSSI" + beacon.getRssi() + ", TxPower" + beacon.getTxPower());
        }
    }

    private void parseJSON(String json) {
        listMinor = new ArrayList<>();
        try {
            JSONObject jsonObj = new JSONObject(json);
            JSONArray jsonArray = jsonObj.getJSONArray("iBeacons");
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject ib = jsonArray.getJSONObject(i);
                listMinor.add(ib.getString("minor_id"));
            }
        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void didEnterRegion(Region region) {
        Log.d("ibeacon", "start : " + region.getId1() + " " + region.getId3());

//        try {
//            beaconManager.startRangingBeaconsInRegion(region);
//        } catch (RemoteException e) {
//            e.printStackTrace();
//        }
    }

    @Override
    public void didExitRegion(Region region) {
        // try {
            // beaconManager.stopRangingBeaconsInRegion(region);
//        } catch (RemoteException e) {
//            e.printStackTrace();
//        }
        Log.d("ibeacon", "didExitRegion");
    }

    @Override
    public void didDetermineStateForRegion(int i, Region region) {
        Log.d("ibeacon", "didDetermineStateForRegion " + region.toString() + " " + region.getId1() + " " + region.getId3());
    }

    @Override
    public void onBeaconServiceConnect() {
        Log.d("ibeacon", "onBeaconServiceConnect");
        beaconManager.addRangeNotifier(this);
//        beaconManager.setMonitorNotifier(new MonitorNotifier() {
//            @Override
//            public void didEnterRegion(Region region) {
//                // 領域への入場を検知
//                Log.d("ibeacon", region.getUniqueId() + " " + region.getId3());
//            }
//
//            @Override
//            public void didExitRegion(Region region) {
//                // 領域からの退場を検知
//            }
//
//            @Override
//            public void didDetermineStateForRegion(int i, Region region) {
//                // 領域への入退場のステータス変化を検知
//            }
//        });
//        beaconManager.addRangeNotifier(new RangeNotifier() {
//            @Override
//            public void didRangeBeaconsInRegion(Collection<Beacon> collection, Region region) {
//                Log.d("ibeacon", "didRangeBeaconsInRegion collection size : " + collection.size());
//                if (collection.size() > 0) {
//                    for (Beacon b : collection) {
//                        for(String minor : listMinor) {
//                            if(b.getId3().toString().equals(minor)) {
//                                notificationManager.notify(notificationId, notification);
//                            }
//                        }
//                    }
//                }
//            }
//        });
    }
}

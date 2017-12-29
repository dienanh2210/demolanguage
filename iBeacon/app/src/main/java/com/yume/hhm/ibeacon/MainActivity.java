package com.yume.hhm.ibeacon;

import android.content.Intent;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import com.yume.hhm.mylibrary.iBeaconLib;


public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        iBeaconLib lb = new iBeaconLib();
        lb.setContext(this);
        lb.turnOnService();
    }
}

package com.fujiwaranosato.YokaiAR.ibeacon;

/**
 * Created by admin on 2/2/2018.
 */

public class IbeaconStruct {
    public String uuid;
    public String major_id;
    public String minor_id;
    public String timeLastShown;
    public IbeaconStruct(String minor, String uid, String major, String times)
    {
        minor_id = minor;
        major_id = major;
        uuid = uid;
        timeLastShown = times;
    }
}

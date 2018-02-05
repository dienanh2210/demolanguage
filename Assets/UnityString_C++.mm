//
//  UnityString_C++.cpp
//  iBeacon
//
//  Created by anhtu on 1/30/18.
//  Copyright © 2018 anhtu. All rights reserved.
//

#include <stdio.h>
#include "iBeacon.h"

extern "C" {
    void start_iBeacon() {
        [iBeacon shared];
    }
    
    void setText(const char *message) {
        [iBeacon setNotificationText:[NSString stringWithUTF8String:message]];
    }
    
    void updateAvailableMinorID (const char *minorIDs) {
        [iBeacon updateAvailableMinorID:[NSString stringWithUTF8String:minorIDs]];
    }
    
    void updateAvailableMajorID (const char * majorIDs) {
        [iBeacon updateAvailableMajorID:[NSString stringWithUTF8String:majorIDs]];
    }
}

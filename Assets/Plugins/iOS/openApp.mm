
//Objective-C Code
#import <Foundation/Foundation.h>

//C-wrapper that Unity communicates with
extern "C"
{
    bool isInstalled(const char *scheme)
    {
        if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:[ [ NSString alloc ] initWithUTF8String:scheme ]]])
        {
            return true;
        }
        return false;
    }
    
    void openByScheme(const char *scheme)
    {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:[ [ NSString alloc ] initWithUTF8String:scheme ]]];
    }
}

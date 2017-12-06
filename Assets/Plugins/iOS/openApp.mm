
//Objective-C Code
#import <Foundation/Foundation.h>    

  @interface SampleClass:NSObject
  /* method declaration */
- (int)isInstalledX;
  @end
  
  @implementation SampleClass


- (int)isInstalledX
   {
        int param = 0;
        NSString *youtubeAppURL = @"youtube://";
		
        if ([[UIApplication sharedApplication] canOpenURL:[NSURL URLWithString:@"youtube://"]])
        {
            param = 1;
			[[UIApplication sharedApplication] openURL:[NSURL URLWithString:youtubeAppURL]];
        }else{
            param = 40;
        }
        
        return param;
        
    }

  @end

 //C-wrapper that Unity communicates with
  extern "C"
  {
      int isInstalled()
     {
        SampleClass *status = [[SampleClass alloc] init];
        return [status isInstalledX];
     }
     
  }
#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>
#import <CoreBluetooth/CBService.h>

// Required by Unity
extern void UnitySendMessage(const char* objectName, const char* methodName, const char* param);

NSString* CreateNSString (const char* string);

@interface JobstickPlugin : NSObject <CBCentralManagerDelegate>
{
	CBCentralManager *CM;
    NSMutableDictionary* m_players;
    int m_maxPlayers;
    bool isBlootuchOn;
}
@end
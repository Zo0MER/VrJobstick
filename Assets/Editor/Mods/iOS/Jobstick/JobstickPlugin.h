#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>
#import <CoreBluetooth/CBService.h>

@interface JobstickPlugin : NSObject <CBCentralManagerDelegate>
{
	CBCentralManager *CM;
    NSMutableDictionary* m_players;
    int m_maxPlayers;
    bool bluetoothEnabled;
}
@end
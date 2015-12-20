//
//
//  Unity-iPhone
//
//  Created by CASE on 01.02.15.
//
//

#import "JobstickPlugin.h"
#include "UnityInterface.h"
#include "JobstickPlayer.h"

// Converts C style string to NSString
NSString* CreateNSString (const char* string)
{
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

// Helper method to create C string copy
char* MakeStringCopy (const char* string)
{
    if (string == NULL)
        return NULL;
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

const char* onBluetoothLENoFind     = "OnBluetoothLENoFind";


@implementation JobstickPlugin

- (id)init
{
    self = [super init];
    if (self)
    {
        m_players = [NSMutableDictionary new];
        [self create];
    }
    return self;
}

-(void)create
{
    CM = [[CBCentralManager alloc] initWithDelegate:self queue:nil];
}

- (void) startScan
{
    [CM scanForPeripheralsWithServices:nil options:0];
}

- (void) stopScan
{
    [CM stopScan];
}


- (void)centralManager:(CBCentralManager *) central didDisconnectPeripheral:(CBPeripheral *)peripheral error:(NSError *)error
{
    //when disconnected try to reconnect
   // [self startScan];
}

- (void)centralManagerDidUpdateState:(CBCentralManager *) central
{
    if(self->CM.state == CBCentralManagerStatePoweredOn)
    {
        isBlootuchOn = true;
        UnitySendMessage("JobstickEventManager", "OnRequestBluetooch", MakeStringCopy("true"));
    }
    
    if(self->CM.state == CBCentralManagerStatePoweredOff)
    {
        isBlootuchOn = false;
        UnitySendMessage("JobstickEventManager", "OnRequestBluetooch", MakeStringCopy("false"));
    }
    
   // if (self->CM.state != CBCentralManagerStatePoweredOn) return;
    //[self startScan];
}

- (void) centralManager:(CBCentralManager *)central didDiscoverPeripheral:(CBPeripheral *)peripheral advertisementData:(NSDictionary *)advertisementData RSSI:(NSNumber *)RSSI {

    if (peripheral.name.length < 8) return; //sometimes some ghost bleutooth devices are found in very noisy area

    if ([peripheral.name rangeOfString:@"Jobstick"].location != NSNotFound && [m_players count] < m_maxPlayers)
    {
        NSUUID* uid = [peripheral identifier];
        NSString* uidString = [uid UUIDString];
        
        if([m_players objectForKey:uidString] == NULL)
        {
            JobstickPlayer* player = [[JobstickPlayer alloc] initWithJobstick:peripheral address:uidString];
            [m_players setObject:player forKey:uidString];
            
            peripheral.delegate = player;
            [CM connectPeripheral:peripheral options:nil];
        }
    }
   
}

- (void) disconnectPeripheralToUID:(NSString *)stringUid
{
    JobstickPlayer* player = [m_players objectForKey:stringUid];
    
    if(player)
    {
        [CM cancelPeripheralConnection:[player jobstick]];
        [m_players removeObjectForKey:stringUid];
    }
}

- (void) disconnectAllPlayers
{
    for(id key in m_players)
    {
        JobstickPlayer* player = [m_players objectForKey:key];
        [CM cancelPeripheralConnection:[player jobstick]];
    }
    
    [m_players removeAllObjects];
}

-(void) setMaxPlayers:(int)max
{
    m_maxPlayers = max;
}

-(void) requestBluetooch
{
    
}

-(bool) bluetoochIsOn
{
    return isBlootuchOn;
}

- (NSString*) getAngless
{
    if([m_players count] > 0)
    {
        id keys = [m_players allKeys];
        JobstickPlayer *player = [m_players objectForKey:[keys objectAtIndex:0] ];
        return [NSString stringWithString:[player getAngles]] ;
    }
    return NULL;
}

- (void)centralManager:(CBCentralManager *) central didConnectPeripheral:(CBPeripheral *) peripheral
{
    [peripheral discoverServices:nil];
}
@end

static JobstickPlugin *jobstickPlugin = nil;



extern "C" 
{
     void _Init()
	 {
		 if (jobstickPlugin == nil)
				jobstickPlugin = [[JobstickPlugin alloc] init];
         [jobstickPlugin setMaxPlayers:2];
	 }

     void _StartScan()
	 {
         [jobstickPlugin startScan];
	 }

     void _StopScan()
	 {
         [jobstickPlugin stopScan];
	 }

     void _DisconnectAllPlayers()
	 {
         [jobstickPlugin disconnectAllPlayers];
	 }

     void _CloseConnectionPlayer(const char* address)
	 {
         [jobstickPlugin disconnectPeripheralToUID:[NSString stringWithUTF8String:address]];
	 }

     void _SetMaxPlayers(int maxPlayers)
	 {
         [jobstickPlugin setMaxPlayers:maxPlayers];
	 }

     void _BluetoothEnable()
	 {
	 }

     void _BluetoothDisable()
	 {
	 }
    
     void _RequestBluetooch()
     {
         [jobstickPlugin create];
     }

     bool _IsBluetoothOn()
	 {
         return [jobstickPlugin bluetoochIsOn];
	 }

     const char* _GetAnglesString()
	 {
         NSString *angles = [NSString stringWithString:[jobstickPlugin getAngless]];
         if([angles length] == 0)
         {
             return 0;
         }

         return MakeStringCopy([angles UTF8String]);
	 }
}
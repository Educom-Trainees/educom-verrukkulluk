import 'react-native-gesture-handler';
import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Image, Text, View, SafeAreaView } from 'react-native';
import { NavigationContainer } from '@react-navigation/native';
import { DrawerContentScrollView, createDrawerNavigator, DrawerItemList } from '@react-navigation/drawer';
import SearchProductScreen from './screens/SearchProduct';
import AddProductScreen from './screens/AddProduct';
import TestScreen from './screens/Test';

const Drawer = createDrawerNavigator();

export default function App() {
  return (
    <NavigationContainer>
      <Drawer.Navigator 
      drawerContent={(props) => <CustomDrawer {...props} />}
      screenOptions={{
        drawerStyle: {
          backgroundColor: 'white',
        },
        headerStyle: {
          backgroundColor: '#95b82d',
        },
        headerTintColor: 'white',
        drawerActiveTintColor: '#95b82d',
      }}>
        <Drawer.Screen name="Overzicht producten" component={SearchProductScreen}/>          
        <Drawer.Screen name="Product toevoegen" component ={AddProductScreen}/>
        <Drawer.Screen name="Test" component ={TestScreen}/>
      </Drawer.Navigator>
    </NavigationContainer>
  );
}

const CustomDrawer = (props) => {
  return (
    <DrawerContentScrollView {...props}>
      <Image
        source={require('./assets/images/verrukkulluk-logo.png')}
        style={styles.drawerImage}
        resizeMode="contain"
      />
      <DrawerItemList {...props} />
    </DrawerContentScrollView>
  )
}

const styles = StyleSheet.create({
  drawerImage: {
    width: 200,
    height: 200,
    alignSelf: 'center',
  },
  drawerText: {
    textAlign: 'center',
    fontsize: 24,
    marginVertical: 5,
  },
});

import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView } from 'react-native';

const AddProductScreen = () => {
    return (
        <View style={styles.container}>
            <Text style={styles.text}>Producten toevoegen!</Text>
        </View>
    );
}

export default AddProductScreen;

const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: '#fff0d6',
      alignItems: 'center',
      justifyContent: 'center',
    },
    safecontainer: {
      flex: 1,
    },
    text: {
        fontSize: 24,
        fontWeight: "bold",
    },
  });
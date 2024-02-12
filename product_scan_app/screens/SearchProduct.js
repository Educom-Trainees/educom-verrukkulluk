import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView } from 'react-native';

const SearchProductScreen = () => {
    return (
        <View style={styles.container}>
            <Text style={styles.text}>Hallo producten!</Text>
        </View>
    );
}

export default SearchProductScreen;

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
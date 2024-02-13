import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, FlatList } from 'react-native';
import ProductCard from '../components/ProductCard';
import productList from '../data.json';
import { createStackNavigator } from '@react-navigation/stack';
import { TouchableOpacity } from 'react-native-gesture-handler';
import ProductDetailsScreen from './ProductDetails';

const SearchProductScreen = () => {
    const Stack = createStackNavigator();

    return (
        <Stack.Navigator>
            <Stack.Screen
                options={{
                    headerShown: false,
                }}
                name="ProductList"
                component={ProductList}
            />
            <Stack.Screen
                options={{
                    headerTitle: '',
                }}
                name="ProductDetails"
                component={ProductDetailsScreen}
            />
        </Stack.Navigator>
    );
};

const ProductList = ({ navigation }) => (
    <SafeAreaView style={styles.container}>
        <FlatList
            style={styles.flatlist}
            data={productList}
            renderItem={({ item }) => (
                <TouchableOpacity onPress={() => navigation.navigate('ProductDetails', { product: item})}>
                    <ProductCard product={item} />
                </TouchableOpacity>
            )}
            keyExtractor={(item) => item.id}
        />
    </SafeAreaView>
);

export default SearchProductScreen;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff0d6',
        alignItems: 'center',
        justifyContent: 'center',
    },
    flatlist: {
        flex: 1,
        width: '95%',
        marginTop: 10,
    },
    safecontainer: {
        flex: 1,
    },
    text: {
        fontSize: 24,
        fontWeight: "bold",
    },
});
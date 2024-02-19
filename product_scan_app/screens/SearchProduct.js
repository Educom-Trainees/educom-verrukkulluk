import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, FlatList } from 'react-native';
import ProductCard from '../components/ProductCard';
import productList from '../data.json';
import { createStackNavigator } from '@react-navigation/stack';
import { TouchableOpacity } from 'react-native-gesture-handler';
import ProductDetailsScreen from './ProductDetails';
import { useEffect, useState } from 'react';

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

const ProductList = ({ navigation }) => {
    const [products, setProducts] = useState([]);
    const ip = 'http://192.168.80.1:45458';

    const getProducts = async () => {
        try {
            const response = await fetch(`${ip}/api/Products`);
            const data = await response.json();
            setProducts(data);
            getProductImages(data);
        } catch (error) {
            console.error(error);
        }
    };

    const getProductImages = async (productArray) => {
        const updatedProducts = await Promise.all(productArray.map(async (product) => {
            try {
                const imageResponse = await fetch(`${ip}/Image/GetImage/${product.imageObjId}`);
                const imageData = await imageResponse.blob();

                return new Promise((resolve, reject) => {
                    const reader = new FileReader();
                    reader.onloadend = () => {
                        resolve({ ...product, image: reader.result});
                    };
                    reader.onerror = reject;
                    reader.readAsDataURL(imageData);
                });
            } catch (error) {
                console.error(`Error fetching image for product ${product.id}: ${error}`);
                return product;
            }
        }));
        setProducts(updatedProducts);
    };

    useEffect(() => {
        getProducts();
    }, []);
    
    return (
        <SafeAreaView style={styles.container}>
            {products.length !== 0 ? (
                <FlatList
                    style={styles.flatlist}
                    data={products}
                    renderItem={({ item }) => (
                        <TouchableOpacity onPress={() => navigation.navigate('ProductDetails', { product: item})}>
                            <ProductCard product={item} />
                        </TouchableOpacity>
                    )}
                    keyExtractor={(item) => item.id}
                />
             ) : (
                <Text style={styles.text}>Geen producten om weer te geven</Text>
             )}
        </SafeAreaView>
    );
};

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
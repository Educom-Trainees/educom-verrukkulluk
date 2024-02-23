import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, FlatList } from 'react-native';
import ProductCard from '../components/ProductCard';
import productList from '../data.json';
import { createStackNavigator } from '@react-navigation/stack';
import { TextInput, TouchableOpacity } from 'react-native-gesture-handler';
import ProductDetailsScreen from './ProductDetails';
import { useEffect, useState } from 'react';
import Constants from 'expo-constants';
import { SearchBar } from 'react-native-elements';

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
    const [searchText, setSearchText] = useState('');
    const ip = 'http://10.0.101.63:45455';

    const getProducts = async () => {
        try {
            const response = await fetch(`${ip}/api/Products`);
            const data = await response.json();
            
            const sortedProducts = data.sort((a, b) => a.name.localeCompare(b.name));

            console.log(sortedProducts);
            setProducts(sortedProducts);
            getProductImages(sortedProducts);
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

    const updateSearch = (search) => {
        setSearchText(search);
    }
    
    return (
        <SafeAreaView style={styles.container}>
            {/*<TextInput
                style={styles.searchbar}
                placeholder='Zoeken'
            />*/}
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
        justifyContent: 'flex-start',
    },
    flatlist: {
        flex: 1,
        width: '95%',
    },
    safecontainer: {
        flex: 1,
    },
    searchbar: {
        height: 100,
        width: '95%',
        backgroundColor: '#ffffff',
    },
    text: {
        fontSize: 24,
        fontWeight: "bold",
    },
});
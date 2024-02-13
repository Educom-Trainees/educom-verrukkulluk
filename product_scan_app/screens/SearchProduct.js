import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, FlatList } from 'react-native';
import ProductCard from '../components/ProductCard';
import productList from '../data.json';
import { useNavigation } from '@react-navigation/native';

const SearchProductScreen = () => {
    const navigation = useNavigation();

    const navigateToProductDetails = (product) => {
        navigation.navigate('ProductDetails', { product });
    };

    return (
        <SafeAreaView style={styles.container}>
            <FlatList style={styles.flatlist}
                data={productList}
                renderItem={({ item }) => <ProductCard product={item} />}
                keyExtractor={(item) => item.id}
            />
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
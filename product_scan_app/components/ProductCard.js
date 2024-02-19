import React from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';

const images = {
    witte_bol: require('../assets/images/witte_bol.jpg'),
    hamburgers: require('../assets/images/hamburgers.png'),
    augurken: require('../assets/images/augurken.jpg'),
}

const ProductCard = ({ product }) => {
    return (
        <View style={styles.card} key={product.id}>
            <View style={styles.cardContent}>
                <Text style={styles.title}>{product.name}</Text>
                {/*<Image source={images[product.picture]} style={styles.image} resizeMode="contain" />*/}
            </View>
        </View>
    );
};

export default ProductCard;

const styles = StyleSheet.create({
    card: {
        backgroundColor: "white",
        flex: 1,
        padding: 16,
        borderRadius: 8,
        borderWidth: 1,
        marginBottom: 10,
    },
    cardContent: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        alignItems: 'center',
    },
    image: {
        width: 100,
        height: 100,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
    },
});
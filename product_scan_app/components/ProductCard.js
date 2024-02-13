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
            <Text>{product.id}</Text>
            <Text>{product.title}</Text>
        </View>
    );
};

export default ProductCard;

const styles = StyleSheet.create({
    card: {
        backgroundColor: "white",
        flex: 1,
        fontSize: 24,
        fontWeight: "bold",
        padding: 16,
        borderRadius: 8,
        borderWidth: 1,
        marginBottom: 10,
    },
    image: {
        width: 100,
        height: 100,
    }
});
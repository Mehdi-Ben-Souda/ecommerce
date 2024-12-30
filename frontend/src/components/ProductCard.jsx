import React from 'react';

const ProductCard = ({ product, onClick }) => {
    return (
        <div className="card card-compact bg-base-100 w-96 shadow-xl transition-transform transform hover:scale-105 cursor-pointer" onClick={onClick}>
            <figure className="relative">
                <img
                    src={product.image}
                    alt={product.name}
                    className="w-full h-48 object-cover"
                />
                <span className="absolute top-2 right-2 bg-white text-gray-800 text-xs font-semibold px-2 py-1 rounded">
                    New
                </span>
            </figure>
            <div className="card-body">
                <h2 className="card-title text-lg font-bold">{product.name}</h2>
                <p className="text-gray-600">{product.description}</p>
                <div className="card-actions justify-between items-center mt-4">
                    <span className="text-xl font-semibold text-green-600">${product.price}</span>
                    <button className="btn btn-primary">Buy Now</button>
                </div>
            </div>
        </div>
    );
}

export default ProductCard;
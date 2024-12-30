import { ShoppingBag } from "lucide-react";
import React from "react";

const EmptyCart = ({ onContinueShopping }) => {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center p-4 bg-base-200">
      <div className="card w-96 bg-base-100 shadow-xl">
        <div className="card-body items-center text-center">
          <ShoppingBag size={48} className="text-base-300 mb-4" />
          <h2 className="card-title text-2xl mb-2">Your cart is empty</h2>
          <p className="text-base-content/70 mb-4">
            Add some items to your cart to get started!
          </p>
          <div className="card-actions">
            <button
              className="btn btn-primary btn-wide"
              onClick={onContinueShopping}>
              Continue Shopping
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default EmptyCart;

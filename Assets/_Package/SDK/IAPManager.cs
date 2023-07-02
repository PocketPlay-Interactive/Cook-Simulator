using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if IAP
using UnityEngine.Purchasing;
#endif

#if IAP
public class IAPManager : MonoSingletonGlobal<IAPManager>, IStoreListener
#else
public class IAPManager : MonoSingletonGlobal<IAPManager>
#endif
{
#if IAP
    private Action Success, Failure;

    private static IStoreController m_StoreController;          // The Unity Purchasing system.
    private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.
    public static string Starter01 = "starter_pack_01";
    public static string Cash01 = "cash_pack_01";
    public static string Cash02 = "cash_pack_02";
    public static string Cash03 = "cash_pack_03";
    public static string Cash04 = "cash_pack_04";
    public static string Cash05 = "cash_pack_05";
    public static string Cash06 = "cash_pack_06";


    private string[] m_ProductNames;

    void Start()
    {
        m_ProductNames = new string[7];
        m_ProductNames[0] = Starter01;
        m_ProductNames[1] = Cash01;
        m_ProductNames[2] = Cash02;
        m_ProductNames[3] = Cash03;
        m_ProductNames[4] = Cash04;
        m_ProductNames[5] = Cash05;
        m_ProductNames[6] = Cash06;

        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(Starter01, ProductType.NonConsumable);
        builder.AddProduct(Cash01, ProductType.Consumable);
        builder.AddProduct(Cash02, ProductType.Consumable);
        builder.AddProduct(Cash03, ProductType.Consumable);
        builder.AddProduct(Cash04, ProductType.Consumable);
        builder.AddProduct(Cash05, ProductType.Consumable);
        builder.AddProduct(Cash06, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyProductID(string productId, Action _success)
    {
#if UNITY_EDITOR
        _success?.Invoke();
#else
        BuyProductID(productId);
        Success = _success;
#endif
    }

    public void RestoreProduct(Action CALLBACK_MISSING, Action CALLBACK_SUCCESS, Action CALLBACK_FAIL)
    {
        if (IsInitialized())
        {
            bool isSuccess = false;
            for (int i = 0; i < m_ProductNames.Length; i++)
            {
                string id = m_ProductNames[i];
                Product product = m_StoreController.products.WithID(id);
                if (product != null && product.hasReceipt)
                {
                    isSuccess = true;
                    switch (id)
                    {
                    }
                }
            }

            if (isSuccess) CALLBACK_SUCCESS?.Invoke();
            else CALLBACK_FAIL?.Invoke();
        }
        else
        {
            Debug.Log("RestoreProductID FAIL. Not initialized.");
            InitializePurchasing();
            CALLBACK_MISSING?.Invoke();
        }
    }

    private void RestoreProductID(string id)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(id);
            if (product != null && product.hasReceipt)
            {
                switch (id)
                {
                }
            }
        }
        else
        {
            Debug.Log("RestoreProductID FAIL. Not initialized.");
            InitializePurchasing();
        }
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
            InitializePurchasing();
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
                if (result)
                {
                    for (int i = 0; i < m_ProductNames.Length; i++)
                    {
                        Debug.Log("Checking store " + m_ProductNames[i]);
                        RestoreProductID(m_ProductNames[i]);
                    }
                }
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }


    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Success?.Invoke();
        return PurchaseProcessingResult.Complete;
    }


    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
#endif
}
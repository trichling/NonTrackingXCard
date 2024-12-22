window.cryptoHelper = {
    generateKeyPair: async function () {
        const keyPair = await window.crypto.subtle.generateKey(
            {
                name: "RSA-OAEP",
                modulusLength: 2048,
                publicExponent: new Uint8Array([1, 0, 1]),
                hash: "SHA-256"
            },
            true,
            ["encrypt", "decrypt"]
        );

        const publicKey = await window.crypto.subtle.exportKey("spki", keyPair.publicKey);
        const privateKey = await window.crypto.subtle.exportKey("pkcs8", keyPair.privateKey);

        return {
            publicKey: btoa(String.fromCharCode(...new Uint8Array(publicKey))),
            privateKey: btoa(String.fromCharCode(...new Uint8Array(privateKey)))
        };
    },
    verifySignature: async function (publicKey, data, signature) {
        const key = await window.crypto.subtle.importKey(
            "spki",
            Uint8Array.from(atob(publicKey), c => c.charCodeAt(0)),
            {
                name: "RSA-PSS",
                hash: { name: "SHA-256" }
            },
            false,
            ["verify"]
        );

        const isValid = await window.crypto.subtle.verify(
            {
                name: "RSA-PSS",
                saltLength: 32
            },
            key,
            Uint8Array.from(atob(signature), c => c.charCodeAt(0)),
            new TextEncoder().encode(data)
        );

        return isValid;
    },
    signData: async function (privateKey, data) {
        const key = await window.crypto.subtle.importKey(
            "pkcs8",
            Uint8Array.from(atob(privateKey), c => c.charCodeAt(0)),
            {
                name: "RSA-PSS",
                hash: { name: "SHA-256" }
            },
            false,
            ["sign"]
        );

        const signature = await window.crypto.subtle.sign(
            {
                name: "RSA-PSS",
                saltLength: 32
            },
            key,
            new TextEncoder().encode(data)
        );

        return btoa(String.fromCharCode(...new Uint8Array(signature)));
    },
     encryptData: async function (publicKey, data) {
        const key = await window.crypto.subtle.importKey(
            "spki",
            Uint8Array.from(atob(publicKey), c => c.charCodeAt(0)),
            {
                name: "RSA-OAEP",
                hash: { name: "SHA-256" }
            },
            false,
            ["encrypt"]
        );

        const encryptedData = await window.crypto.subtle.encrypt(
            {
                name: "RSA-OAEP"
            },
            key,
            new TextEncoder().encode(data)
        );

        return btoa(String.fromCharCode(...new Uint8Array(encryptedData)));
    },
    decryptData: async function (privateKey, encryptedData) {
        const key = await window.crypto.subtle.importKey(
            "pkcs8",
            Uint8Array.from(atob(privateKey), c => c.charCodeAt(0)),
            {
                name: "RSA-OAEP",
                hash: { name: "SHA-256" }
            },
            false,
            ["decrypt"]
        );

        const decryptedData = await window.crypto.subtle.decrypt(
            {
                name: "RSA-OAEP"
            },
            key,
            Uint8Array.from(atob(encryptedData), c => c.charCodeAt(0))
        );

        return new TextDecoder().decode(decryptedData);
    },
    hashData: async function (data) {
        const hashBuffer = await window.crypto.subtle.digest(
            "SHA-256",
            new TextEncoder().encode(data)
        );
        return btoa(String.fromCharCode(...new Uint8Array(hashBuffer)));
    }
};
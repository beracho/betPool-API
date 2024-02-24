export async function fetchData(url, options) {
    var data = null;
    var error = false;

    const baseURL = `https://localhost:7152/api`

    await fetch(baseURL + url, options)
        .then(res => {
            if (!res.ok) {
                throw Error("Error in process");
            }
            return res;
        })
        .then(dataResponse => {
            data = dataResponse;
        })
        .catch(err => {
            error = err.message;
        })

    return { data, error }
}
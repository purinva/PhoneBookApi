function Pagination({page, setPage}) {
    return (
        <div className="mt-4 flex justify-between items-center">
            <button
                onClick={() => setPage(page - 1)}
                disabled={page === 1}
                className="px-4 py-2 bg-blue-500 text-white 
                rounded disabled:bg-gray-400"
                >
                Назад
            </button>
            <button
                onClick={() => setPage(page + 1)}
                className="px-4 py-2 bg-blue-500 text-white rounded"
                >
                Дальше
            </button>
      </div>
    );
}

export default Pagination;
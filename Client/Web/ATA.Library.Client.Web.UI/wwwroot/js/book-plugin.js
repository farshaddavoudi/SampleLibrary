function flipBook(bookId) {
    let $book = $("#" + bookId);
    var $parent = $book.parent(),
        $page = $book.children('div.bk-page'),
        $bookview = $parent.find('button.bk-bookview'),
        $content = $page.children('div.bk-content'),
        current = 0;

        $bookview.removeClass('bk-active');

        if ($book.data('flip')) {

            $book.data({ opened: false, flip: false }).removeClass('bk-viewback').addClass('bk-bookdefault');

        } else {

            $book.data({ opened: false, flip: true }).removeClass('bk-viewinside bk-bookdefault').addClass('bk-viewback');

        }
}







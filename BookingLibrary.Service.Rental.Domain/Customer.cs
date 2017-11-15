using System;
using System.Collections.Generic;
using BookingLibrary.Domain.Core;
using BookingLibrary.Service.Rental.Domain.Events;
using System.Linq;

namespace BookingLibrary.Service.Rental.Domain
{
    public class Customer : AggregateRoot,
    IHandler<BookRentedEvent>,
    IHandler<BookReturnedEvent>,
    IHandler<CustomerAccountInitializedEvent>,
    IHandler<RentBookRequestCreatedEvent>
    {
        public Customer()
        {

        }

        public Customer(Guid customerId, PersonName name)
        {
            ApplyChange(new CustomerAccountInitializedEvent
            {
                AggregateId = customerId,
                Name = name
            });
        }

        public PersonName Name { get; set; }

        public List<Guid> Books { get; internal set; }

        public void Handle(BookRentedEvent evt)
        {
            this.Books.Add(evt.BookInventoryId);
        }

        public void Handle(BookReturnedEvent evt)
        {
            this.Books.RemoveAll(p => p == evt.BookId);
        }

        public void Handle(CustomerAccountInitializedEvent evt)
        {
            this.Books = new List<Guid>();
            this.Name = evt.Name;
            this.Id = evt.AggregateId;
        }

        public void RentBookRequest(Book book)
        {
            ApplyChange(new RentBookRequestCreatedEvent
            {
                ISBN = book.ISBN,
                BookName = book.BookName,
                BookInventoryId = book.Id,
                RentDate = DateTime.Now,
                AggregateId = this.Id,
                Name = this.Name
            });
        }

        public void RentBook(Guid bookInventoryId)
        {
            ApplyChange(new BookRentedEvent
            {
                AggregateId = this.Id,
                BookInventoryId = bookInventoryId
            });
        }

        public void ReturnBook(Guid bookId)
        {
            ApplyChange(new BookReturnedEvent
            {
                BookId = bookId,
                ReturnDate = DateTime.Now,
                AggregateId = this.Id
            });
        }

        public void Handle(RentBookRequestCreatedEvent evt)
        {
            
        }
    }
}
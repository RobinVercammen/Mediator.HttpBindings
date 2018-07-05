import { IRequest } from '../irequest';

export class <class.TypeName>: IRequest\<<first(class.Extends).TypeName>\<<first(first(class.Extends).GenericArguments)>>>{
  <class.Properties :{property | <property.Name>: <property.TypeName><if(property.ArrayLike)>[]<endif><\n>}>
}